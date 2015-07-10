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
	When I save "localhost/Newworkflow"
	Then "Newworkflow" is visible in "localhost"


Scenario: Saving a Workflow in localhost folder
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I open "Folder 1" in save dialog 
	When I save "Newworkflow"
	Then "Newworkflow" is visible in "localhost/Folder 1"	

	
Scenario: Save button is Enabled when I enter new name for resource and filter
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	Then save button is "Disabled"
	And Filter is "Folder 1"
	Then I should see "1" folders
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


Scenario: Cancel Saving a Workflow in localhost folder
	Given the Save Dialog is opened
	And the "localhost" server is visible in save dialog
	And I open "Folder 1" in save dialog 
	When I enter name "NewWorkflow"
	Then save button is "enabled"
	When I cancel
	Then "NewWorkflow" is not visible in "Folder 1"

Scenario: Save with Filter
	Given the Save Dialog is opened
	When I refresh the filter
	Then "Folder 1" is visible in "localhost"
