# Real-Time Migration of Pega to Dataverse
> When faced with the scenario of migrating an existing application from the Pega platform to a Dataverse solution, there is often a misconception that this is no possible due to Pega's proprietary method of handling case data.
This solution details the necessary steps to create a fault-tolerant, scalable migration pattern that provides maximum flexibility.

## Pega Overview 

Pega applications support a complex object model that allows for great flexibility in building UI and Business Process Flows. Key to understanding this migration pattern is a basic understanding of how Pega stores data.

    * Pega leverages both data class and work classes.
    * Data classes can be complex objects as well, but ultimately can support being written out to 
    external table/s.
    * Work objects, or cases, are inherently complex objects that contain specific instantiations of 
    multiple data objects & classes
    * To support an ever-changing, highly complex data model in the work class, the work object itself 
    is stored within a single column of the corresponding PegaData table, in a proprietary BLOB format.
    * Pega provides a tool called BIX, that can be used to extract individual objects to a table structure, 
    and while discussed as part of the overall solution, due to its complexity and time-consuming development, 
    it is not a core part of the solution discussed.

## Solution Explanation

The pattern implemented in this scenario allows for maximum fault-tolerance and flexibility in migration timelines. As one-time data sync can be error prone, requiring down-time and precise cutover steps, this approach allows both Pega and Dataverse to synchronize in real-time & stay in-sync as end users are migrated over. There are several core technologies used in this process.
    * Azure CosmosDB - This NoSQL database is used to maintain an always up-to-date copy of the Pega case object.
    * Azure Service Bus - Used for messaging, to control when Dataverse should be updated
    * Azure Functions - Provides a scalable, cost-effective middleware API layer that can easily scale 
    to handle Pega's high volume of DB transactions.
    * Azure Logic Apps - Provides a no-code interface for mapping the Pega case data into Dataverse
    * Dataverse - provides access to the migrated data via its highly extensible, low-code/no-code tool set.

   ![Pega Migration Pattern](/images/MigrationPattern.png)

## Solution Steps

 In this pattern, a Declare Trigger is used "On Committed Save" which means that every time a specific class is committed to the database within Pega, the integration activity is called. A REST connector should be created within Pega to connect to your Azure Function web services. (Additionally, you can add Azure Blob Storage as the repository for your Pega application, so all attachments will be natively stored within your MSFT tenant.)

The Azure Function creates or updates the JSON document for the case within CosmosDB and places a message on the service bus queue.

The Logic app retrieves the message from the queue and creates or updates the necessary entities within Dataverse, automatically mapping contacts to cases, etc.

Note, that while not included in this solution, BIX extracts of data objects can also be used in conjunction with the real-time sync approach.