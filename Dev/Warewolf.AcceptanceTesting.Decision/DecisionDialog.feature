﻿Feature: DecisionDialog
	In order to create a decision
	As a Warewolf User
	I want to be shown the decision window setup


#WOLF-1082 
Scenario Outline: Ensure Inputs are enabled on decision window load
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	Then the Decision window is opened
	And '<Inputs>' fields are "Enabled"
	And an empty row has been added
	And the decision match variables '<Variable>'and match '<Variable2>' and to match'<Variable3>'
	And MatchType  is '<MatchType>'
	Then the inputs are '<Inputs>'
	Examples: 
	| Inputs                             | Variable | Variable2 | Value2 | Variable3 | Value3 | MatchType       |
	| Visible, Visible, Visible          | [[a]]    | [[b]]     | 12     |           |        | <               |
	| Visible, Visible                   | [[a]]    |           |        |           |        | is Alphanumeric |
	| Visible, Visible, Visible, Visible | [[a]]    | [[b]]     | 5      | [[c]]     | 15     | Is Between       |


Scenario: Ensuring decision text is visible under tool
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	And the Decision window is opened
	And a decision variable "[[A]]" value "123 234"		
	And is "[[A]]" "IsEqual" "123   234"	
	And "Done" is selected
	Then the Decision tool window is closed
	And "[[A]] = 123 234" is visible in tool
	
Scenario: Ensure Decision window caches correctly
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	And a decision variable "[[A]]" value "123 234"	
	And a decision variable "[[B]]" value "'1'"	
	And "Require All Decisions To Be True" is "True"
	And the Decision window is opened	
	When I change decision variable "[[B]]" to "3111"
	And "Done" is selected
	Then I open the Decision tool window
	And decision variable "[[B]]" is not visible
	And "3111" is visible in Match field
	And "Require All Decisions To Be True" is "True"	


Scenario: Ensure statement line can be removed
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	And a decision variable "[[A]]" value "123 234"	
	And a decision variable "[[B]]" value "1"
	And Match Type equals ">"
	And a decision variable "[[c]]" value "Lester"
	And a decision variable "[[d]]" value "L"	
	And Match Type equals "Starts With"	
	And "Require All Decisions To Be True" is "True"
	And I right click to view the context menu
	And I select "remove statement line"
	And "[[c]]" "Starts With" "[[d]]" is removed from the decision
	And "Done" is selected
	And the decision has "No" error


Scenario: Validation on incorrectly formatted variables
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	And a decision variable "[[A]]}" value ""
	And a decision variable "[[rec().a]]" value "28/08/2015"	
	And Match Type equals "="
	And "Done" is selected
	And the decision tool has "An" Error
	And Error message "incorrect match variable "[[A]]}" is visible


Scenario Outline: Ensure Match Type droplist is populated correctly
	Given I have a workflow "New Workflow"
	And drop a "Decision" tool onto the design surface
	And the Decision window is opened
	And I select the "Match Type" menu
	And Match Type has '<options>' visible
	Examples: 
	| options            |
	| Choose...          |
	| There is An Error  |
	| There is No Error  |
	| =                  |
	| >                  |
	| <                  |
	| <>                 |
	| >=                 |
	| <=                 |
	| Starts With        |
	| Ends With          |
	| Contains           |
	| Doesn't Start With |
	| Doesn't End With   |
	| Doesn't Contain    |
	| Is Alphanumeric    |
	| Is Base64          |
	| Is Between         |
	| Is Binary          |
	| Is Date            |
	| Is Email           |
	| Is Hex             |
	| Is Numeric         |
	| Is RegEx           |
	| Is Text            |
	| Is Xml             |
	| Not Alphanumeric   |
	| Not Base64         |
	| Not Between        |
	| Not Binary         |
	| Not Date           |
	| Not Email          |
	| Not Hex            |
	| Not Numeric        |
	| Not RegEx          |
	| Not Text           |
	| Not Xml            |