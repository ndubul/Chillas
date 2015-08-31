Feature: SwitchDialog
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

#Wolf-1086
@ignore
Scenario: Using switch tool
	Given I have a workflow "New Workflow"
	And drop a "Switch" tool onto the design surface
	Then the Switch tool window is opened
	And "Variable to Switch on" is "Enabled"
	And "Display text" is "Enabled"
	And "Done" is "Enabled"
	And "Cancel" is "Enabled"
	And "Variable to Switch on" equals "4"
	And "Display text" equals "Four"
	And "Done" is selected
	And the Switch tool window is closed

Scenario: Assigning value to switch arms
	Given I have a workflow "New Workflow"
	And drop a "Switch" tool onto the design surface
	Then the Switch tool window is opened
	And "Variable to Switch on" equals "4"
	And "Display text" equals "Four"
	And "Done" is selected
	And the switch tool window is closed
