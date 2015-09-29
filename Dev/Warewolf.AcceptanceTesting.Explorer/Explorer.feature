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
Scenario: Opening and Editing workflow from Explorer localhost
	Given the explorer is visible
	And I open "localhost" server
	And I create the "localhost/Hello World" of type "WorkflowService" 
	When I open 'Hello World' in "localhost"
	And "Hello World" tab is opened

@Explorer
Scenario: Opening and Editing workflow from Explorer Remote
	Given the explorer is visible
	And I connect to "Remote Connection Integration" server
	And I open "Remote Connection Integration" server
	And I create the "Remote Connection Integration/Hello World" of type "WorkflowService" 
	When I open 'Hello World' in "Remote Connection Integration"
	And "Hello World" tab is opened 
	

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
Scenario: Move Nested Folder up tree-view Remote
	Given the explorer is visible
	And I connect to "Remote Connection Integration" server
	And I open "Remote Connection Integration"
	When I create "Remote Connection Integration/Testing"
	When I create "Remote Connection Integration/Testing/ForEach"
	And I change path "Remote Connection Integration/Testing/ForEach" to "Remote Connection Integration/ForEach"
	Then I should see the path "Remote Connection Integration/Testing" 
	Then I should see the path "Remote Connection Integration/ForEach" 

@Explorer
Scenario: Move Nested Folder up tree-view 
	Given the explorer is visible
	And I open "localhost"
	When I create "localhost/MyFolder"
	When I create "localhost/MyFolder/NewFolder"
	And I change path "localhost/MyFolder/NewFolder" to "localhost"
	Then I should see the path "localhost/MyFolder" 
	Then I should see the path "localhost/NewFolder" 

@Explorer
Scenario: Checking versions in remote connection 
  Given the explorer is visible
  And I connect to "Remote Connection Integration" server
  When I open "Remote Connection Integration" server
  And I create the "Remote Connection Integration/Folder 1/Resource 1" of type "WorkflowService" 
  And I Setup  "3" Versions to be returned for "Remote Connection Integration/Folder 1/Resource 1"
  When I Show Version History for "Remote Connection Integration/Folder 1/Resource 1"
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

#wolf-996
Scenario: Context menu
	Given the explorer is visible
	When I right-click on "Localhost"
	Then the context menu appears as
	| Menu Items     |
	| New Folder     |
	| Server Version |
	And I right-click on "Local/Examples"
	Then the context menu appears as
	| Menu Items                |
	| New Workflow Service      |
	| New Server Source         |
	| New Database Connector    |
	| New Database Source       |
	| New Web Service Connector |
	| New Web Service Source    |
	| New Plugin Connector      |
	| New Plugin Source         |
	| New Email Source          |
	| New Dropbox Source        |
	| New Sharepoint Source     |
	| New Folder                |
	| Rename                    |
	| Delete                    |
	| Deploy                    |
	| Show Dependencies         |

	
#wolf-996
Scenario: Disconnected from remote server
	Given the explorer is visible
	When I connect to "Remote Connection Integration" server
	And I open "Remote Connection Integration" server
	Then I should see "10" folders
	Then I should see the path "Remote Connection Integration/Folder 2"
	When I click "Disconnect"
	Then "Remote Connection Integration" is "Disconnected"
	And "Localhost" is visible