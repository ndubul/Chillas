@SaveDialog
Feature: SaveDialog
	In order to save resources
	As a Warewolf user
	I want a save dialog

Scenario: Creating Folder from Save Dialog under localhost
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders
	When I create "New Folder" in "localhost"
	Then I should see "6" folders

#CODED UI
#Scenario: Right click Items on folder
#    Given the Save Dialog is opened 
#	And the "localhost" server is visible in save dialog
#	And I should see "5" folders in "localhost" save dialog
#	When I right click on "folder 1"
#	Then I should see "Rename"
#	And I should see "Delete"
#	And I should see "New Folder"
#	And I shouldn't see "New workflow service"
#	And I shouldn't see "New Plugin service"


Scenario: Saving a Workflow in localhost
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I should see "5" folders
	When I save "localhost/Newworkflow"
	Then "NewWorkflow" is visible in "localhost"


Scenario: Saving a Workflow in localhost folder
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I open "Folder 1" in save dialog 
	When I save "Folder 1/Newworkflow"
	Then "NewWorkflow" is visible in "Folder 1"	

	
Scenario: Save button is Enabled when I enter new name for resource and filter
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	Then "Save" is "disabled"
	And Filter is "Folder 1"
	Then I should see "1" folders"
	And I open "Folder 1" in save dialog 
	When I enter name "Savewf"
	Then save button is "enabled"
	And validation message is ""

Scenario: Save with duplicate name and expect validation
    Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	When I open "Folder 2" in save dialog 
	When I enter name "Folder 2 Child 1"
	Then save button is "Disabled"
	And validation message is "An item with name 'Folder 2 Child 1' already exists in this folder."

Scenario: Save resource names with special character expect validation
    Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	When I open "Folder 1" in save dialog 
	When I enter name "Save@#$"
	Then save button is "Disabled"
	And validation message is "'Name' contains invalid characters."


Scenario: Opening New workflow and saving
    Given I have a New workflow "Unsaved 1" open
	When I press "Ctrl+s"
	Given the Save Dialog is opened
	Then "Save" is "disabled"
	And "Cancel" is "enabled"
	When I enter name "New"
	Then save button is "Enabled"
	When I save "localhost/New"
	Then the "New" workflow is saved "True"


Scenario: Opening Save dialog and canceling
    Given I have a New workflow "Unsaved 1" a open
	When I press "Ctrl+s"
	Given the Save Dialog is opened
	And cancel button is "Enabled"
	When I click "Cancel"
	Then the save dialog is closed
	Then the "New" workflow is saved "False"


Scenario: Saving multiple workflows by using shortcutkey
    Given I have a New workflow "Unsaved 1"  open
	And I have a New workflow "Unsaved 2" open
	And I have a "Saved1" workflow open
	And I have a "Saved2" workflow open
	And I have a "Saved3" workflow open
	When I press "Ctrl+Shift+S"
	Then the "Saved1" workflow is saved "True" 
	Then the "Saved2" workflow is saved "True" 
	Then the "Saved3" workflow is saved "True" 
    And the Save Dialog is opened
	And the Save Dialog title is "Save: Unsaved 1" 
	And the title connection is "localhost (http://computername:3142/dsf)
	When I cancel the save dialog
	And the Save Dialog is opened 
	And the Save Dialog title is "Save: Unsaved 2" 
	And the title connection is "localhost (http://computername:3142/dsf)
	When I enter name "New"
	Then save button is "Enabled"
	When I save "localhost/New"
	Then the "New" workflow is saved "True"


Scenario: Save Doesnt Allow Nested Duplicate
    Given I have an New workflow "Unsaved 1" is open
	When I press "Ctrl+s"
	Then the Save Dialog is opened
	When I select "localhost/Folder 1"
	When I open "Folder 1" in save dialog 
	Then I should see "8" children for "Folder 1"
	When I select "localhost/Folder 1/children1"
	Then save name is "children1"
	When I save "localhost/children1"
	Then validation message is thrown "True"
	Then validation message is "Name already exists"
	Then the "children1" workflow is saved "False"


Scenario: Star is representing the workflow is unsaved
   Given I have an New workflow "Unsaved 1" is open 
   When I edit "Unsaved 1"
   Then the New workflow "Unsaved 1" is open with Star notation
   When I save "Unsaved 1" as "New Workflow"
   Then the New workflow "New Workflow" is open without Star notation
   And "Unsaved 1" is "invisible"
   When I edit "New Workflow"
   Then I have an "New Workflow" workflow open with Star notation
   When I save "New Workflow"
   Then I have an "New Workflow" workflow without Star notation