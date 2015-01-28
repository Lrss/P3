#define _CRT_SECURE_NO_DEPRECATE
#define Export extern "C" __declspec(dllexport)
#include <fstream>
#include <string>
#include <iostream>
#include <vector>
#include <opencv2\opencv.hpp>
#include <opencv2\highgui\highgui.hpp>

using namespace cv;
using namespace std;

int ArraySize = 25;//Amount of bytes to reserve memory for.
int location[9];//Array to store X, Y values for both players.

const int MIN_AREA = 1728;//MIN object area
const int MAX_AREA = 115200;//MAX object area

Scalar P1_BGRmin;
Scalar P1_BGRmax;
Scalar P2_BGRmin;
Scalar P2_BGRmax;
Scalar HSVmin;
Scalar HSVmax;

bool arrowRelease(Rect roi, Mat cameraFrame, Rect rect)
{
	int totalbrightness = 0;

	//tmp will have the actual camera frame cut in half, presenting either the right or left half.
	//Depending on which rectangle is passed into the function in trackFilteredObject(rectLeft or rectRight)
	Mat tmp = cameraFrame(rect);

	//Tmp2 will then use the ROI found in the findROI function, to further isolate the actual camera footage, into a ROI
	//only containing the actual power controller.
	Mat tmp2 = tmp(roi);

	//The ROI is converted to the HSV colorspace, as we want to detect differences in brightness.
	cvtColor(tmp2, tmp2, CV_BGR2HSV);

	//We use a for loop to run through each pixel of the image, adding up the brightness values(Channel 2 of each individual pixel)
	for (int j = 0; j < tmp2.rows; j++)
	for (int i = 0; i < tmp2.cols; i++)
		totalbrightness += tmp2.at<Vec3b>(j, i)[2];


	//If the total brightness is over a set threshold, the LEDs on the controllers are on, and the arrow should be released.
	if (totalbrightness > 300000)
		return true;//Arrow shot
	else
		return false;//Arrow not shot
}

Rect findROI(Mat roiFrame)
{
	//First we create a rectangle to represent the region of interest, and then a vector of points to store the different points
	//That represent the controller
	Rect box;
	vector<Point> points;

	//We use an iterator to go through the image, looking for white pixels. If a pixel is white, its position is recorded in the vector.
	Mat_<uchar>::iterator it = roiFrame.begin<uchar>();
	Mat_<uchar>::iterator end = roiFrame.end<uchar>();

	for (; it != end; ++it)
	if (*it)
		points.push_back(it.pos());
	//If the vector has some points present in it, the box is set to be a bounding rectangle, that surrounds these points.
	if (points.size() > 0)
		box = boundingRect(points);

	return box;
}

//trackFilteredObject(playerOne, threshold1, cameraFeed);

//This function is the same as the one used in calibration mode, but it takes thes controller objects, that have been
//Seperated by thresholding as input.
void trackObjects(string ControllerType, Mat threshold, Mat &cameraFeed)
{
	//these vectors are needed to save the output of findCountours
	vector< vector<Point> > contours;
	vector<Vec4i> hierarchy;

	//Find the contours of the image
	findContours(threshold, contours, hierarchy, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE);

	//Moments are used to find the filtered objects.
	int x, y;
	if (hierarchy.size() > 0)
	{
		int numObjects = hierarchy.size();
		//If there are more objects than the maximum number of objects we want to track, the filter may be noisy.
		if (numObjects < 3)
		{
			for (int i = 0; i >= 0; i = hierarchy[i][0])
			{
				Moments moment = moments((Mat)contours[i]);
				double area = moment.m00;
				//This finds objects, if area is lesser than min_area it is probably noise
				if (area > MIN_AREA)
				{
					x = moment.m10 / area;//Player X
					y = moment.m01 / area;//Player Y
				}
			}
		}
	}

	if (ControllerType == "playerOne")
	{
		location[0] = x;//Left player X
		location[1] = y;//Left player Y
	}
	if (ControllerType == "playerTwo")
	{
		location[4] = x;//Right player X
		location[5] = y;//Right player Y
	}

	//If the controller type used in trackFilteredObject is a power controller, we want to do some extra work with it.
	if (ControllerType == "powerController")
	{
		//We create rectangles that are used to cut up the pictures into two half region of interests. Divided down the middle.
		Rect rectLeft = Rect(0, 0, cameraFeed.cols / 2, cameraFeed.rows);
		Rect rectRight = Rect(cameraFeed.cols / 2, 0, cameraFeed.cols / 2, cameraFeed.rows);

		/*We now take the thresholded image and divide it up into the left and right half,
		using the previously created rectangles, and pass it to findROI function,
		which finds the actual player controller, and uses the position
		To define a new region of interest.*/
		Rect leftROIBox = findROI(threshold(rectLeft));
		location[2] = leftROIBox.x + leftROIBox.width / 2;//Left power X
		location[3] = leftROIBox.y + leftROIBox.height / 2;//Left power Y

		Rect rightROIBox = findROI(threshold(rectRight));
		location[6] = rightROIBox.x + rightROIBox.width / 2 + rectRight.width;//Right power X
		location[7] = rightROIBox.y + rightROIBox.height / 2;//Right power Y

		//The new ROI for the actual power controller is passed into the arrowRelease function, which detects whether or not the arrow is to be released.
		if (arrowRelease(leftROIBox, cameraFeed, rectLeft))
			location[8] = 1;
		else
			location[8] = 0;
		if (arrowRelease(rightROIBox, cameraFeed, rectRight))
			location[8] += 2;
	}
}

void morphOps(Mat &thresh)
{
	//Create a structuring element to be used for morph operations.
	Mat structuringElement = getStructuringElement(MORPH_RECT, Size(4, 4));
	Mat dilateElement = getStructuringElement(MORPH_RECT, Size(5, 5));

	//Perform the morphological operations, using two/three iterations because the noise is horrible.
	erode(thresh, thresh, structuringElement, Point(-1, -1), 2);
	dilate(thresh, thresh, dilateElement, Point(-1, -1), 1);
	erode(thresh, thresh, structuringElement, Point(-1, -1), 1);
}

void loadConfig(string fileName)
{
	ifstream calibrationFile;
	int lineCount = 0;
	calibrationFile.open(fileName);

	if (calibrationFile)
	{
		string line;
		string textFile[12];
		/*
		From the calibration program code we know that every other line is the value that we want to convert to an int,
		and save to our controller object. Line numbers start as 0 ascend.
		So line 1 = B_MIN, line 3 = B_MAX, line 5 = G_MIN etc.
		Same goes for the HSV values, line 1 = H_MIN, line 3 = H_MAX etc.
		*/
		while (getline(calibrationFile, line))
		{
			textFile[lineCount] = line;
			lineCount++;
		}

		if (fileName == "greenConfig.txt"){
			P1_BGRmin = Scalar(stoi(textFile[1]), stoi(textFile[5]), stoi(textFile[9]));
			P1_BGRmax = Scalar(stoi(textFile[3]), stoi(textFile[7]), stoi(textFile[11]));
		}
		if (fileName == "redConfig.txt"){
			P2_BGRmin = Scalar(stoi(textFile[1]), stoi(textFile[5]), stoi(textFile[9]));
			P2_BGRmax = Scalar(stoi(textFile[3]), stoi(textFile[7]), stoi(textFile[11]));
		}

		if (fileName == "powerConfig.txt"){
			HSVmin = Scalar(stoi(textFile[1]), stoi(textFile[5]), stoi(textFile[9]));
			HSVmax = Scalar(stoi(textFile[3]), stoi(textFile[7]), stoi(textFile[11]));
		}
	}
}

Export int DLLArraySize()
{
	return ArraySize;
}

Export  void OpenCVData(char * PlayerData)
{
	Mat cameraFeed, HSV, threshold1, threshold2, threshold3;

	loadConfig("greenConfig.txt");
	loadConfig("redConfig.txt");
	loadConfig("powerConfig.txt");
		
	//Store an initial frame
	//capture.read(cameraFeed);//To run with camera
	cameraFeed = cvLoadImage("img.png");//To run without camera

	//If not in calibrationmode, threshold the image based on the defined min max values for player 1, 2 and power controller.
	//Save to seperate thresholded images.
	inRange(cameraFeed, P1_BGRmin, P1_BGRmax, threshold1);
	inRange(cameraFeed, P2_BGRmin, P2_BGRmax, threshold2);

	//Convert the frame to HSV colorspace
	cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);

	//Again, the powercontrollers are thresholded using the HSV colorspace.
	inRange(HSV, HSVmin, HSVmax, threshold3);

	//Perform morphology on each image
	morphOps(threshold1);
	morphOps(threshold2);
	morphOps(threshold3);

	//Track player one, two and power controller.
	trackObjects("playerOne", threshold1, cameraFeed);
	trackObjects("playerTwo", threshold2, cameraFeed);
	trackObjects("powerController", threshold3, cameraFeed);

	string DataString;
	//Converts int array to one string.
	for (int i = 0; i < 8; i++){
		//Fills zeroes in so each location uses 3 char's of space no matter what.
		if (to_string(location[i]).length() < 3){
			if (to_string(location[i]).length() < 2){
				DataString += "00" + to_string(location[i]);
			}
			else
				DataString += "0" + to_string(location[i]);
		}
		else
			DataString += to_string(location[i]);
	}
	DataString += to_string(location[8]);

	//Converts string to char array.
	for (int i = 0; i < ArraySize; i++)
		PlayerData[i] = DataString[i];
	/* 
	location[0] Left player X
	location[1] Left player Y
	location[2] Left power X
	location[3] Left power Y
	location[4] Right player X
	location[5] Right player Y
	location[6] Right power X
	location[7] Right power Y
	location[8] Is arrow shot
	*/
	return;
}