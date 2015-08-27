Feature: DecisionDialog
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

#WOLF-1082 
Scenario Outline: Ensure Inputs are enabled on decision window load
	Given I have a workflow "New Workflow"
	And drop a "Decision" onto the design surface
	Then the Decision window is opened
	And '<Inputs>' fields are "Enabled"
	And "Add Statement" is visible
	And a decision variable '<variable>' value '<value>'
	And a decision variable '<variable2>' value '<value2>'
	And a decision variable '<variable3>' value '<value3>'
	And check if '<variable>' "<MatchType>" '<variable2>'
	When the decision tool is executed
	Then the execution has "NO" error
	Examples: 
	| Inputs                         | Variable | Value | Variable2 | Value2 | Variable3 | Value3 | MatchType       |
	| Match, MatchType, Match        | [[a]]    | 10    | [[b]]     | 12     |           |        | <               |
	| Match, MatchType               | [[a]]    | 10    |           |        |           |        | is Alphanumeric |
	| Match, MatchType, Match, Match | [[a]]    | 10    | [[b]]     | 5      | [[c]]     | 15     | isBetween       |


Scenario: Ensuring decision text is visible under tool
	Given a decision variable "[[A]]" value "123 234"		
	And is "[[A]]" "IsEqual" "123   234"	
	And "Done" is selected
	Then the Decision tool window is closed
	And "[[A]] = 123 234" is visible in tool
	
Scenario: Ensure Decision window caches correctly
	Given a decision variable "[[A]]" value "123 234"	
	And a decision variable "[[B]]" value "1"	
	And "Require All Decisions To Be True" is "True"	
	And I change decision variable "[[B]]" to "3111"
	And "Done" is selected
	When I open the Decision tool window
	Then decision variable "[[B]]" is not visible
	And "Require All Decisions To Be True" is "True"	


Scenario: Ensure statement line can be removed
	Given a decision variable "[[A]]" value "123 234"	
	And a decision variable "[[B]]" value "1"
	And Match Type equals ">"
	And a decision variable "[[c]]" value "Lester"
	And a decision variable "[[d]]" value "Lance"
	And Match Type equals "Starts With"	

