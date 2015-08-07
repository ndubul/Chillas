Feature: DropboxSource
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: New Dropbox Source
	Given I have open New Dropbox Source
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
