# LogRetriever
Gather Logs from Fidelis Endpoint

The Log Retriever application provides the ability to gather needed logging for troubleshooting quickly and easily for support. The application allows you to choose which log files to retrieve while also allowing you to specify the amount of days in the past to search.

Instructions for use:

using the move buttons >> and << you can highlight and move files between the available logs and logs to archive list. Once a file is moved from one list to the other it will not appear in the other list. This keeps the system from creating duplicate logs in the final archive.

The Days text box allows you to specify the amount of days in the past to retrieve logs. It defaults to 30 as that is a pretty standard amount of time for any issue that would require log retrieval. Setting the value blank will allow the retrieval of all logs in the logging folders no matter the date.

To enable log retrieval on distributed installs of the Fidelis Endpoint Application you must have do three things:

The LogRetriever.exe.config in the same folder as the logretriever.exe
The DistributedInstall Key in the LogRetriever.exe.config must be set to true. This tells the application that it needs to search other computers for the logs. 
You will also need to update the configuration file with the names of the servers depending on what is installed. The servers needed by their componenents are below:

MAPServer: Where the Map componenent is installed
WCFServer: Where the WCF services are installed 
SiteServer: Where the Site Server is installed 
CollectionWorkManager: Where the Work Manager that uses the site server is installed 
ProcessingServer: Where the Evidence Processor is installed

Also to go along with the Processing Server is the path to the Processing Logs. As this is a path that can be customized during install, it will need to be defined if it is on a different server.

If some application pieces are on the same server as the MAP then leave those as localhost.

To retrieve the logs from your local workstation, just simply configure the Configuration file with the server names and you will be able to pull directly to your machine without having to access the server.
