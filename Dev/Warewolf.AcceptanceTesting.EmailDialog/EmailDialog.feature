Feature: EmailDialog
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


# Send Email with an attachment
# Selecting multiple attachments
@ignore
#wolf - 991
Scenario: Send Email with an attachment
	Given the "from" account is "warewolf@dev2.co.za"
	And to address is "test1@freemail.com" 	
	And I  want to attach an item
	When I expand the Email tool
	And I click "Attachments"
	Then the webs file chooser dialog opens 

Scenario: Selecting multiple attachments
	Given the Email dialog is opened
	And I navigate to "C:\Temp\Testing" 
	And I attach "C:\Temp\Testing\test.txt"
	And I navigate to "E:\AppData\Le"
	And I attach "E:\AppData\Le\test.txt"
	Then files to attach should appear as "C:\Temp\Testing\test.txt;E:\AppData\Le\test.txt"


Scenario: Ensure that dialog tree view is populated correctly
	Given the Email dialog is opened
	And all network drives are visible
	And I expand "C:\"
	Then all the folders in "C:\" are visible



