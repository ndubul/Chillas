﻿@Explorer
Feature: Explorer
	In order to manage my service
	As a Warewolf User
	I want explorer view of my resources with management options

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
	Then I should see "Folder 3" only
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

#WOLF-1025
@ignore
Scenario: Creating Folder in remote host
   Given the explorer is visible
   When I open "Remote Connection Integration" server
   Then I should see "66" folders
   When I add "MyNewFolder" in "Remote Connection Integration"
   Then I should see the path "Remote Connection Integration/MyNewFolder" 


Scenario: Opening and Editing workflow from Explorer
	Given the explorer is visible
	And I open "localhost" server
	When I open 'Resource 1' in "localhost/Folder 1"
	And "Hello World" tab is opened 	

Scenario: Renaming Folder And Workflow Service on a remote server
	Given the explorer is visible
	And I open "Remote Connection Integration" server
	When I rename "Remote Connection Integration/Folder 2" to "Test Rename"
	Then I should see "1" child for "Test Rename"
	When I open "Test Rename"
	And I create the "Remote Connection Integration/Test Rename/New Test 1" of type "WorkflowService" 
	Then I should see the path "Remote Connection Integration/Test Rename/New Test 1"
	And I should see "2" children for "Test Rename"
	And I should not see the path "Remote Connection Integration/Folder 2" in explorer

Scenario: Context menu
	Given the explorer is visible
	And I open "localhost" server
	Then I should see "5" folders
	When I right click on "LocalHost" a context menu is visible
	And "New Folder" is visible
	And "New Service" is visible
	And "New Database Connector" is visible
	And "New Plugin Connector" is visible
	And "New Web Service Connector" is visible
	And "New Remote Warewolf Source" is visible
	And "New Database Source" is visible
	And "New Plugin Source" is visible
	And "New Web Service Source" is visible
	And "New Email Source" is visible
	And "New Dropbox Source" is visible
	And "New Sharepoint Source" is visible
	And "Server version"

Scenario: Show dependencies
	Given the explorer is visible
	And I open "Remote Connection Integration"
	And I right click on "Hello World" a context menu is visible
	And I select "Show Dependencies"
	Then the Show Dependencies tab is opened
	And "Hello World" is visible with all dependencies

Scenario: Open saved Server Sources
	Given the explorer is visible
	And I open "Remote Connection Integration"
	Then path "Remote Connection Integration/Server" is visible
	And I open "Remote Connection Integration/Server/Trav"
	Then the "Trav" server tab is opened

Scenario: Move Nested Folder up tree-view
	Given the explorer is visible
	And I open "localhost"
	Then path "localhost/MyFolder/NewFolder" is visible
	And I change path "localhost/MyFolder/NewFolder" to "localhost/NewFolder"
	Then both "localhost/MyFolder" and "localhost/NewFolder" are visible in root


Scenario: Opening server source from explorer
	Given the explorer is visible
	And I open "LocalHost"
	Then I should see the path "localhost/tst-ci-remote"
	And I open "localhost/tst-ci-remote"
	Then "Edit - tst-ci-remote" tab is opened

	

#@Explorer
#Scenario: Opening Versions in Explorer
#  Given the explorer is visible
#  When I open "localhost" server
#  And I create the "localhost/Folder 1/Resource 1" of type "WorkflowService" 
#  Then I should see the path "localhost/Folder 1/Resource 1"
#  And I Setup  "3" Versions to be returned for "localhost/Folder 1/Resource 1"
#  #Testing Resource Icons  
# # #Opening Version History
# When I Show Version History for "localhost/Folder 1/Resource 1"
# Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"
# # And I should not see "3" versions with "View,Execute" Icons
# # When I open "v.1" of "WF1" in "Folder 1"
# # Then "v.1" is opened
#  When I Make "localhost/Folder 1/Resource 1/v.1" the current version of "localhost/Folder 1/Resource 1" 
# Then I should see "4" versions with "View" Icons in "localhost/Folder 1/Resource 1"
# # #Deleting Versions
# When I Delete Version "localhost/Folder 1/Resource 1/v.1"
# # Then I should not see "v.2"
# Then I should see "3" versions with "View" Icons in "localhost/Folder 1/Resource 1"
#
#
#Scenario: No Version history option for services and sources.
#  Given the explorer is visible
#  When I open "localhost" server
#  And I Setup a resource  "1" "WebService" to be returned for "localhost" called "WebService"
#  And I Add  "1" "PluginService" to be returned for "localhost"
#  And I Add  "1" "ServerSource" to be returned for "localhost"
#  Then I should see the path "localhost/WebService"
#  Then I should see the path "localhost/PluginService"
#  Then I should see the path "localhost/ServerSource"
#  And "Show Version History" Context menu  should be "Invisible" for "localhost/WebService 1"
#  And "Show Version History" "localhost/PluginService" should be "Invisible" for "localhost/Webservice"
#  And "Show Version History" "localhost/Remoteserver" should be "Invisible" for "localhost/Webservice"

# 
#
#
#Scenario: Creating Services Under Localhost 
#	Given the explorer is visible
#	When I open "New Service" in "localhost" server
#	Then "Unsavesd1" is opened
#	When I open "New Database Connector" in "localhost" server
#	Then "New Database Service" is opened
#	When I open "New Plugin Connector" in "localhost" server
#	Then "New Plugin Service" is opened
#	When I open "New Web Service Connector" in "localhost" server
#	Then "New Web Service" is opened
#	When I open "New Remote Warewolf Source" in "localhost" server
#	Then "New Server" is opened
#	When I open "New Plugin Source" in "localhost" server
#	Then "New Plugin Source" is opened
#	When I open "New Web Source" in "localhost" server
#	Then "New Web Source" is opened
#	When I open "New Email Source" in "localhost" server
#	Then "New Email Source" is opened
#	When I open "New Dropbox Source" in "localhost" server
#	Then "Dropbox Source" is opened
#
#
#cenario: Creating Services Under Explorer Folder 
#	Given the explorer is visible
#	When I open "New Service" for "Folder1" in "localhost" server
#	Then "Unsavesd1" is opened
#	When I open "New Database Connector" for "Folder1" in "localhost" server
#	Then "New Database Service" is opened
#	When I open "New Plugin Connector" for "Folder1" in "localhost" server
#	Then "New Plugin Service" is opened
#	When I open "New Web Service Connector" for "Folder1" in "localhost" server
#	Then "New Web Service" is opened
#	When I open "New Remote Warewolf Source" for "Folder1" in "localhost" server
#	Then "New Server" is opened
#	When I open "New Plugin Source" for "Folder1" in "localhost" server
#	Then "New Plugin Source" is opened
#	When I open "New Web Source" for "Folder1" in "localhost" server
#	Then "New Web Source" is opened
#	When I open "New Email Source" for "Folder1" in "localhost" server
#	Then "New Email Source" is opened
#	When I open "New Dropbox Source" for "Folder1" in "localhost" server
#	Then "Dropbox Source" is opened
#	When I Deploy "Folder 1" of "localhost" server
#	Then "Deploy" is opened
#
#
#
#cenario: Context Menu Items for workflow.
#	Given the explorer is visible
#	When I open "Open" for "workflow" in "localhost" server
#	Then "workflow" is opened
#	And I open "New Database Connector" for "workflow" in "localhost" server is "False"
#	And I open "New Plugin Connector" for "workflow" in "localhost" server is "False"
#	And I open "New Web Service Connector" for "workflow" in "localhost" server is "False"
#	And I open "New Remote Warewolf Source" for "workflow" in "localhost" server is "False"
#	And I open "New Plugin Source" for "workflow" in "localhost" server is "False"
#	And I open "New Web Source" for "workflow" in "localhost" server is "False"
#	And I open "New Email Source" for "workflow" in "localhost" server is "False"
#	And I open "New Dropbox Source" for "workflow" in "localhost" server is "False"
#	And I Deploy "workflow" of "localhost" server
#	And "Deploy" is opened
#	And I open Dependencies of "workflow" in "localhost" server
#	And "Ones*Dependants" is opened
#
#
#cenario: Opening Dependencies Of All Services In Explorer
#   Given the explorer is visible
#	When I open Show Dependencies of "WF1" in "Folder1"
#	Then "WF1 Dependents" is opened
#	When I open Show Dependencies of "WebServ1" in "Folder1"
#	Then "WebServ1 Dependents" is opened
#	When I open Show Dependencies of "DB Service1" in "Folder1"
#	Then "DB Service1 Dependents" is opened
#	When I open Show Dependencies of "PluginServ1" in "Folder1"
#	Then "PluginServ1 Dependents" is opened
#
#



#
#Scenario: Renaming Service in explorer
#   Given the explorer is visible
#	And I open "localhost" server
#	And I should see "Renameresource" in "localhost"
#	When I rename "Renameresource" to "renamed" in "localhost" server
#	Then I should see "renamed" in "localhost" server
#	Then Conflict message should be occured
#
#
#Scenario: Rename conflicting resources
#   Given the explorer is visible
#	And I open "localhost" server
#	And I should see "Conflict" in "localhost"
#	And I should see "Renameresource" in "localhost"
#	When I rename "Conflict" to "Renameresource" in "localhost" server
#	Then I should see "renameconflict" in "localhost" server
#	Then Conflict message should be occured
#
#
#
#	
#********************************Requires server side interaction. Normal specs or Coded UI **********************************
#Scenario: Renaming Workflow Service Is Creating Version In Version History
#	Given the explorer is visible
#	And I open "localhost" server
#	Given I rename "WF2" of "Follder 1" to "WorkFlow2" in "Localhost" server 
#	Then I should see "WorkFlow2" of "Folder1" in "localhost" server
#	And I should not see "WF2" in "Folder1"
#	When I Show Version History for "WF2" in "Folder 1"
#   Then I should see "1" versions with "View" Icons
#   And I should not see "1" version with "Execute" Icons
#
#

# *************** To be done with remote Servers ****************************
#Scenario: Opening Resources from remote server
#   Given the explorer is visible
#	When I Connected to Remote Server "Remote"
#	And I open "Remote" server
#	Then I should see "10" folders
#	
#

