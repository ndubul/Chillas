@Explorer
Feature: Explorer
	In order to manage my service
	As a Warewolf User
	I want explorer view of my resources with management options

# Connected to localhost server
# Expand a folder
# Rename folder
# Search explorer
# Creating Folder in localhost
# Creating And Deleting Folder and Popup says cancel in localhost
# Deleting Resource in folders
# Deleting Resource in localhost Server
# Renaming Folder And Workflow Service
# Searching resources by using filter
# Checking versions 
# Clear filter
# Search explorer on remote server
# Connected to remote server
# Creating Folder in remote host
# Opening and Editing workflow from Explorer
# Renaming Folder And Workflow Service on a remote server
# Context menu
# Show dependencies
# Open saved Server Sources
# Move Nested Folder up tree-view
# Opening server source from explorer
# Move Nested Folder up tree-view
# Opening server source from explorer
# Show Server Version

@Explorer
Scenario: Connected to localhost server
	Given the explorer is visible
	When I open "localhost" server
	Then I should see "5" folders

@Explorer
Scenario: Expand a folder
	Given the explorer is visible
	And I open "localhost" server
	When I open "Folder 2"
	Then I should see "18" children for "Folder 2"

@Explorer
Scenario: Rename folder
	Given the explorer is visible
	And I open "localhost" server
	When I rename "localhost/Folder 2" to "Folder New"
	Then I should see "18" children for "Folder New"
	Then I should see the path "localhost/Folder New" 
	Then I should not see the path "localhost/Folder 2" 

@Explorer
Scenario: Search explorer
	Given the explorer is visible
	And I open "localhost" server
	When I search for "Folder 3"
	Then I should see "localhost/Folder 3" only
	And I should not see "Folder 1"
	And I should not see "Folder 2"
	And I should not see "Folder 4"
	And I should not see "Folder 5"

@Explorer
Scenario: Creating Folder in localhost
   Given the explorer is visible
   When I open "localhost" server
   Then I should see "5" folders
   When I add "MyNewFolder" in "localhost"
   Then I should see the path "localhost/MyNewFolder" 


@Explorer  
Scenario: Creating And Deleting Folder and Popup says cancel in localhost
  Given the explorer is visible
  When I open "localhost" server
  Then I should see "5" folders
  When I add "MyOtherNewFolder" in "localhost"
  Then I should see the path "localhost/MyOtherNewFolder" 
  And I should see "6" folders
  And I choose to "Cancel" Any Popup Messages
  Then I should see "6" folders
  When I open "Folder 2"
  Then I should see "18" children for "Folder 2"
  When I create "localhost/Folder 2/myTestNewFolder"
  Then I should see "19" children for "Folder 2"
  Then I should see the path "localhost/Folder 2/myTestNewFolder"
  And I choose to "OK" Any Popup Messages
  When I delete "localhost/Folder 2/myTestNewFolder"
  Then I should see "18" children for "Folder 2" 
  Then I should not see the path "localhost/Folder 2/myTestNewFolder"

@Explorer
Scenario: Deleting Resource in folders
   Given the explorer is visible
   When I open "localhost" server
   Then I should see "5" folders
   When I open "Folder 5"
   And I create the "localhost/Folder 5/deleteresource" of type "WorkflowService" 
   Then I should see the path "localhost/Folder 5/deleteresource"
   When I delete "localhost/Folder 5/deleteresource"
   Then I should not see "deleteresouce" in "Folder 5"

@Explorer
Scenario: Deleting Resource in localhost Server
   Given the explorer is visible
   When I open "localhost" server
   And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
   Then I should see "5" folders
   Then I should see the path "localhost/Folder 1/Resource 1"
   When I delete "localhost/Folder 1/Resource 1"
   Then I should not see the path "localhost/Folder 1/Resource 1"

@Explorer
Scenario: Renaming Folder And Workflow Service
	Given the explorer is visible
	And I open "localhost" server
	When I rename "localhost/Folder 2" to "Folder New"
	Then I should see "18" children for "Folder New"
	When I open "Folder New"
	And I create the "localhost/Folder New/Resource 1" of type "WorkflowService" 
	And I create the "localhost/Folder New/Resource 2" of type "WorkflowService" 
	Then I should see the path "localhost/Folder New"
	Then I should see the path "localhost/Folder New/Resource 1"
	And I should not see "Folder 2"
	And I should not see the path "localhost/Folder 2"
	When I rename "localhost/Folder New/Resource 1" to "WorkFlow1"	
	Then I should see the path "localhost/Folder New/WorkFlow1"
	When I rename "localhost/Folder New/Resource 2" to "WorkFlow1"	
	Then Conflict error message is occurs

@Explorer
Scenario: Searching resources by using filter
  Given the explorer is visible
  And I open "localhost" server
  When I open "Folder 1"
  And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
  Then I should see the path "localhost/Folder 1/Resource 1"
  When I search for "Folder 1" in explorer
  Then I should see the path "localhost/Folder 1"
  Then I should not see the path "localhost/Folder 1/Resource 1"
  Then I should not see the path "localhost/Folder 2"
  When I search for "Resource 1" in explorer
  When I open "Folder 1"
  Then I should see the path "localhost/Folder 1/Resource 1"

@Explorer
Scenario: Checking versions 
  Given the explorer is visible
  When I open "localhost" server
  And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
  Then I should see "5" folders
  And I Setup  "3" Versions to be returned for "localhost/Folder 1/Resource 1"
  When I Show Version History for "localhost/Folder 1/Resource 1"
  Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"
  When I search for "Resource 1" in explorer
  Then I should see the path "localhost/Folder 1/Resource 1"
  Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"

@Explorer
Scenario: Clear filter
  Given the explorer is visible
  And I open "localhost" server
  When I open "Folder 1"
  And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
  Then I should see the path "localhost/Folder 1/Resource 1"
  When I search for "Folder 1" in explorer
  Then I should see the path "localhost/Folder 1"
  Then I should not see the path "localhost/Folder 1/Resource 1"
  Then I should not see the path "localhost/Folder 2"
  When I search for "Resource 1" in explorer
  When I open "Folder 1"
  Then I should see the path "localhost/Folder 1/Resource 1"
  When I clear "Explorer" Filter
  Then I should see the path "localhost/Folder 2"
  Then I should see the path "localhost/Folder 2"
  Then I should see the path "localhost/Folder 2"
  Then I should see the path "localhost/Folder 2"

@Explorer
Scenario: Search explorer on remote server
	Given the explorer is visible
	And I connect to "Remote Connection Integration" server
	And I open "Remote Connection Integration" server
	And I create the "Remote Connection Integration/Hello World" of type "WorkflowService" 
	Then I should see "10" folders
	When I search for "Hello World"
	Then I should see "Remote Connection Integration/Hello World" only

@Explorer
Scenario: Connected to remote server
	Given the explorer is visible
	When I connect to "Remote Connection Integration" server
	And I open "Remote Connection Integration" server
	Then I should see "10" folders
	Then I should see the path "Remote Connection Integration/Folder 2"

@Explorer
Scenario: Creating Folder in remote host
   Given the explorer is visible
   And I connect to "Remote Connection Integration" server
   And I open "Remote Connection Integration" server
   And I should see "10" folders
   When I add "MyNewFolder" in "Remote Connection Integration"
   Then I should see the path "Remote Connection Integration/MyNewFolder" 

@Explorer
Scenario Outline: Opening and Editing workflow from Explorer
	Given the explorer is visible
	And I open "<Host>" server
	And I create the "<Host>/Hello World" of type "WorkflowService" 
	When I open "Hello World"
	And "Hello World" tab is opened 
	Examples: 
	| Host                          |
	| localhost                     |
	| Remote Connection Integration |

@Explorer
Scenario: Renaming Folder And Workflow Service on a remote server
	Given the explorer is visible
	And I connect to "Remote Connection Integration" server
    And I open "Remote Connection Integration" server
	When I rename "Remote Connection Integration/Folder 2" to "Folder New"
	Then I should see "18" children for "Folder New"
	When I open "Folder New"
	And I create the "Remote Connection Integration/Folder New/Resource 1" of type "WorkflowService" 
	And I create the "Remote Connection Integration/Folder New/Resource 2" of type "WorkflowService" 
	Then I should see the path "Remote Connection Integration/Folder New"
	Then I should see the path "Remote Connection Integration/Folder New/Resource 1"
	And I should not see the path "Remote Connection Integration/Folder 2"
	When I rename "Remote Connection Integration/Folder New/Resource 1" to "WorkFlow1"	
	Then I should see the path "Remote Connection Integration/Folder New/WorkFlow1"
	When I rename "Remote Connection Integration/Folder New/Resource 2" to "WorkFlow1"	
	Then Conflict error message is occurs

@Explorer
Scenario: Show dependencies
	Given the explorer is visible
	And I open "Remote Connection Integration"
	And I right click on "Hello World" a context menu is visible
	And I select "Show Dependencies"
	Then the Show Dependencies tab is opened
	And "Hello World" is visible with all dependencies

@Explorer
Scenario: Open saved Server Sources
	Given the explorer is visible
	And I open "Remote Connection Integration"
	Then I should see the path "Remote Connection Integration/Server"
	And I open "Remote Connection Integration/Server/Trav"
	Then the "Trav" server tab is opened

@Explorer
Scenario Outline: Move Nested Folder up tree-view
	Given the explorer is visible
	And I open "<Host>"
	Then path "<path>" is visible
	And I change path "<From>" to "<To>"
	Then both "<Folder1>" and "<Folder2>" are visible
	Examples: 
	| Host                          | path                                          | From                                          | To                                    | Folder1                               | Folder2                               |
	| LocalHost                     | localhost/MyFolder/NewFolder                  | localhost/MyFolder/NewFolder                  | localhost/NewFolder                   | localhost/MyFolder                    | localhost/NewFolder                   |
	| Remote Connection Integration | Remote Connection Integration/Testing/ForEach | Remote Connection Integration/Testing/ForEach | Remote Connection Integration/ForEach | Remote Connection Integration/ForEach | Remote Connection Integration/Testing |  

@Explorer
Scenario Outline: Opening server source from explorer
	Given the explorer is visible
	And I open "<Host>"
	Then I should see the path "<path>"
	And I open "<path>"
	Then "<Hostname>" tab is opened
	Examples:
	| Host                          | path                                    | HostName      |
	| LocalHost                     | localhost/tst-ci-remote                 | tst-ci-remote |
	| Remote Connection Integration | Remote Connection Integration/Sandbox-1 | Sandbox       |

@Explorer
Scenario: Checking versions in remote connection 
  Given the explorer is visible
  When I open "Remote Connection Integration" server
  And I create the "Remote Connection Integration/Folder 1/Resource 1" of type "WorkflowService" 
  Then I should see "67" folders
  And I Setup  "3" Versions to be returned for "Remote Connection Integration/Folder 1/Resource 1"
  When I Show Version History for "Remote Connection Integration/Folder 1/Resource 1"
  Then I should see "3" versions with "View" Icons in "Remote Connection Integration/Folder 1/Resource 1"
  When I search for "Resource 1" in explorer
  Then I should see the path "Remote Connection Integration/Folder 1/Resource 1"
  Then I should see "3" versions with "View" Icons in "Remote Connection Integration/Folder 1/Resource 1"

@Explorer
Scenario: Opening Versions in Explorer
  Given the explorer is visible
  When I open "localhost" server
  And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
  Then I should see the path "localhost/Folder 1/Resource 1"
  And I Setup  "3" Versions to be returned for "localhost/Folder 1/Resource 1"
 When I Show Version History for "localhost/Folder 1/Resource 1"
 Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"
  When I Make "localhost/Folder 1/Resource 1/v.1" the current version of "localhost/Folder 1/Resource 1" 
 Then I should see "4" versions with "View" Icons in "localhost/Folder 1/Resource 1"
 When I Delete Version "localhost/Folder 1/Resource 1/v.1"
 Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"


Scenario: No Version history option for services and sources.
  Given the explorer is visible
  When I open "localhost" server
  And I Setup a resource  "1" "WebService" to be returned for "localhost" called "WebService"
  And I Add  "1" "PluginService" to be returned for "localhost"
  And I Add  "1" "ServerSource" to be returned for "localhost"
  Then I should see the path "localhost/WebService"
  Then I should see the path "localhost/PluginService"
  Then I should see the path "localhost/ServerSource"
  And "Show Version History" Context menu  should be "Invisible" for "localhost/WebService 1"
  And "Show Version History" "localhost/PluginService" should be "Invisible" for "localhost/Webservice"
  And "Show Version History" "localhost/Remoteserver" should be "Invisible" for "localhost/Webservice"

 
Scenario: Opening Dependencies Of All Services In Explorer
   Given the explorer is visible
	When I open Show Dependencies of "WF1" in "Folder1"
	Then "WF1 Dependents" is opened
	When I open Show Dependencies of "WebServ1" in "Folder1"
	Then "WebServ1 Dependents" is opened
	When I open Show Dependencies of "DB Service1" in "Folder1"
	Then "DB Service1 Dependents" is opened
	When I open Show Dependencies of "PluginServ1" in "Folder1"
	Then "PluginServ1 Dependents" is opened