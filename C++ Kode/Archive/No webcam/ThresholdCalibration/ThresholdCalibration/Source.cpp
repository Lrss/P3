#include <opencv2\core\core.hpp>
#include <opencv2\highgui\highgui.hpp>
#include <opencv2\opencv.hpp>
#include <iostream>
#include <fstream>
#include <string>

using namespace cv;
using namespace std;

//Initialize min and max values for the HSV thresholding
int H_MIN = 0;
int H_MAX = 255;
int S_MIN = 0;
int S_MAX = 255;
int V_MIN = 0;
int V_MAX = 255;
//Initialize min and max values for BGR thresholding
int B_MIN = 0;
int B_MAX = 255;
int G_MIN = 0;
int G_MAX = 255;
int R_MIN = 0;
int R_MAX = 255;
//Names for the different config files
const string greenCfg = "greenConfig.txt";
const string redCfg = "redConfig.txt";
const string powerCfg = "powerConfig.txt";

void on_trackbar(int, void*)
{
}

//This function just prints some helping commands to the console. Only called once when the program is first run
void help()
{
	cout
		<< "This program is to perform the calibrations on a static image\n\n"
		<< "The sliders are used to find the relevant BGR and HSV values for use in \nthresholding.\n\n"
		<< "The program uses the following hotkeys: \n"
		<< "Press Q to toggle between BGR and HSV thresholding.\n"
		<< "Press 1 to save the values for the green controller.\n"
		<< "Press 2 to save the values for the red controller. \n"
		<< "Press 3 to save the values for the power(blue) controller.\n";
}

//These are the trackbars used for BGR thresholding.
void BGRTrackbars()
{
	namedWindow("BGR Trackbars", 0);
	char BGRTrackbar[6];
	sprintf(BGRTrackbar, "B_MIN", B_MIN);
	sprintf(BGRTrackbar, "B_MAX", B_MAX);
	sprintf(BGRTrackbar, "G_MIN", G_MIN);
	sprintf(BGRTrackbar, "G_MAX", G_MAX);
	sprintf(BGRTrackbar, "R_MIN", R_MIN);
	sprintf(BGRTrackbar, "R_MAX", R_MAX);

	createTrackbar("B_MIN", "BGR Trackbars", &B_MIN, B_MAX, on_trackbar);
	createTrackbar("B_MAX", "BGR Trackbars", &B_MAX, B_MAX, on_trackbar);
	createTrackbar("G_MIN", "BGR Trackbars", &G_MIN, G_MAX, on_trackbar);
	createTrackbar("G_MAX", "BGR Trackbars", &G_MAX, G_MAX, on_trackbar);
	createTrackbar("R_MIN", "BGR Trackbars", &R_MIN, R_MAX, on_trackbar);
	createTrackbar("R_MAX", "BGR Trackbars", &R_MAX, R_MAX, on_trackbar);
}

//Trackbars used for HSV thresholding
void HSVTrackbars()
{
	namedWindow("HSV Trackbars", 0);
	char HSVTrackbar[6];
	sprintf(HSVTrackbar, "H_MIN", H_MIN);
	sprintf(HSVTrackbar, "H_MAX", H_MAX);
	sprintf(HSVTrackbar, "S_MIN", S_MIN);
	sprintf(HSVTrackbar, "S_MAX", S_MAX);
	sprintf(HSVTrackbar, "V_MIN", V_MIN);
	sprintf(HSVTrackbar, "V_MAX", V_MAX);

	createTrackbar("H_MIN", "HSV Trackbars", &H_MIN, H_MAX, on_trackbar);
	createTrackbar("H_MAX", "HSV Trackbars", &H_MAX, H_MAX, on_trackbar);
	createTrackbar("S_MIN", "HSV Trackbars", &S_MIN, S_MAX, on_trackbar);
	createTrackbar("S_MAX", "HSV Trackbars", &S_MAX, S_MAX, on_trackbar);
	createTrackbar("V_MIN", "HSV Trackbars", &V_MIN, V_MAX, on_trackbar);
	createTrackbar("V_MAX", "HSV Trackbars", &V_MAX, V_MAX, on_trackbar);
}

//Used to perform morphological operations on the relevant frame.
void morphOps(Mat thresh)
{
	//Create the structuring element used in the morphological operations
	Mat erodeElement = getStructuringElement(MORPH_RECT, Size(4, 4));
	Mat dilateElement = getStructuringElement(MORPH_RECT, Size(5, 5));
	//Perform the operations
	erode(thresh, thresh, erodeElement, Point(-1, -1), 2);
	dilate(thresh, thresh, dilateElement, Point(-1, -1), 1);
	erode(thresh, thresh, erodeElement, Point(-1, -1), 1);
}

void writeValues(string filename)
{
	//Create a ofstream (output file stream) class that can handle the output from the program, to the text file.
	ofstream calibrationFile;
	//Open/create the relevant file, depending on which string variable was passed into the function
	//fstream::trunc makes sure the file is empty before writing the values further down the code.
	calibrationFile.open(filename, fstream::out |fstream::trunc);
	//If for some reason the file failed to open, function doesn't go any further.
	if (!calibrationFile)
	{
		cout << "Failed to open relevant calibration file." << endl;
		exit(1);
	}
	//The config for the power controller needs a different set of values that are in the HSV colorspace.
	//So in this case, the HSV values are written instead of BGR.
	if (filename == powerCfg)
	{
		calibrationFile
			<< "H_MIN = \n" << H_MIN << endl
			<< "H_MAX = \n" << H_MAX << endl
			<< "S_MIN = \n" << S_MIN << endl
			<< "S_MAX = \n" << S_MAX << endl
			<< "V_MIN = \n" << V_MIN << endl
			<< "V_MAX = \n" << V_MAX << endl;
	}
	else {
		calibrationFile
			<< "B_MIN = \n" << B_MIN << endl
			<< "B_MAX = \n" << B_MAX << endl
			<< "G_MIN = \n" << G_MIN << endl
			<< "G_MAX = \n" << G_MAX << endl
			<< "R_MIN = \n" << R_MIN << endl
			<< "R_MAX = \n" << R_MAX << endl;
	}
	//Ensures that the file is properly closed once the program is done writing.
	calibrationFile.close();
}

void readValues(string search)
{
	int foundVariable;
	string line;
	ifstream calibrationFile;
	size_t pos;
	calibrationFile.open("calibration.txt");
	if (!calibrationFile)
	{
		cout << "Unable to open file" << endl;
		exit(1);
	}
	while (getline(calibrationFile, line))
	{
		pos = line.find(search);
		if (pos != string::npos)
		{
			cout << line << endl;
			getline(calibrationFile, line);
			foundVariable = stoi(line);
			cout << foundVariable;
		}
	}
	calibrationFile.close();
}


int main()
{
	//Call the help function to make sure people know how to work the program.
	help();

	//Create the Mat(image) variables to be used throughout the code.
	Mat cameraFeed, HSV, BGRThreshold, HSVThreshold;
	//int cameraPort;
	//HSV mode is set to false by default, so the program begins with BGR thresholding. This can be switched at any time during the calibration process.
	bool HSVMode = false;

	//cout << "\nSelect which camera to use, values are 0 or 1. \nMost likely 0 for the built in camera, and 1 for an external camera.\n";
	
	//cin >> cameraPort;
	//Set the variables to determine the size of the capture frame
	const int FRAME_WIDTH = 480;
	const int FRAME_HEIGHT = 360;
	//Open the camera for recording.
	//VideoCapture capture(cameraPort);
	//Set the dimensions of the captured frame
	//capture.set(CV_CAP_PROP_FRAME_WIDTH, FRAME_WIDTH);
	//capture.set(CV_CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT);
	cameraFeed = cvLoadImage("img.png");
	/*
	If the trackbars are created within the while loop(the function is called each time it runs) the trackbars go nuts. 
	Therefore it is assumed that the default mode is for HSV mode to be off, and the trackbars for BGR Thresholding are created. 
	After this point creation and destruction of trackbar windows is handled on keyinput at the end of the while loop 
	*/
	if (!HSVMode)
		BGRTrackbars();

	//help_this_function_is_shit();
	//Infinite loop used to continually show the webcam feed
	while (1)
	{

		//Stor the initial frame
		//capture.read(cameraFeed);
		imshow("Original Image", cameraFeed);
		if (!HSVMode)
		{
			//Inrange performs the thresholding operation, looking through each pixel of the frame. If the pixels values are between the min and max threshold
			//The pixel is made white on the output image.
			inRange(cameraFeed, Scalar(B_MIN, G_MIN, R_MIN), Scalar(B_MAX, G_MAX, R_MAX), BGRThreshold);
			morphOps(BGRThreshold);
			imshow("BGR Thresholding", BGRThreshold);
		}
		if (HSVMode)
		{
			//The frame is converted to the HSV colorspace
			cvtColor(cameraFeed, HSV, COLOR_BGR2HSV);
			//Thresholding is performed as described previously.
			inRange(HSV, Scalar(H_MIN, S_MIN, V_MIN), Scalar(H_MAX, S_MAX, V_MAX), HSVThreshold);
			morphOps(HSVThreshold);
			imshow("HSV Thresholding", HSVThreshold);	
		}
		//Switch statement that waits for 30ms, this is to allow the user to input the keys to save values, or change to HSV thresholding
		//The delay is also necessary in order for the program to consistently output new frames.
		switch (waitKey(30))
		{
		case 'q':
			switch (HSVMode)
			{
			case 0:
				destroyWindow("BGR Trackbars");
				destroyWindow("BGR Thresholding");
				HSVTrackbars();
				HSVMode = true;
				break;
			case 1:
				destroyWindow("HSV Trackbars");
				destroyWindow("HSV Thresholding");
				BGRTrackbars();
				HSVMode = false;
				break;
			}
			break;
		case '1':
			writeValues(greenCfg);
			cout << "Green controller values saved" << endl;
			break;
		case '2':
			writeValues(redCfg);
			cout << "Red controller values saved" << endl;
			break;
		case '3':
			writeValues(powerCfg);
			cout << "Power controller values saved" << endl;
			break;
			//27 = the escape key
		case 27:
			return 0;
			break;
		}
	}
}