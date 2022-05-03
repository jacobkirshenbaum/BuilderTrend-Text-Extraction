# BuilderTrend-Text-Extraction
This application will be used to extract text using Apache tika from various different files
and store it in an Elasticsearch client for later searching. 

# Release Notes:
This application is able to extract text from various file types using Apache Tika and Tesseract 
OCR. There is also an alternative method for extracting text using GroupDocs that is only able 
to extract text from up to 100 files. Along with that, it can put this text into Elasticsearch 
so that it may be searched for at a later time. There is also the ability to upload and download 
the files to and from Google Cloud Storage.

#Installation:
To run this application, you just have to run the Program.cs file. This will open up the user
interface and allow you to use any of the functionalities mentioned above. For it to work properly
you will need the Google Cloud service account credentials file to access the files we currently 
have in it. To make the project work in your own environment you must change the credentials in 
the App.config file to match your Elasticsearch instances. For your own Google Cloud Storage 
instances you will need to update the GoogleStorage.cs file to have the JsonPath point to your
Google Cloud service account credentials and the BucketName to match the bucket you intend to use.
