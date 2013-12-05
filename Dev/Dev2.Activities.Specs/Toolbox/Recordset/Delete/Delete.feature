﻿Feature: Delete
	In order to delete records
	As a Warewolf user
	I want a tool that takes a record set and deletes it

Scenario: Delete last record in a recordset 
	Given I have the following recordset
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |
	And I delete a record "[[rs()]]"
	When the delete tool is executed
	Then the delete result should be "Success"
	And the recordset "[[rs().row]]" will be as follows
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |

Scenario: Delete a recordset that does not exist
	Given I have the following recordset
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |
	And I delete a record "[[rd()]]"
	When the delete tool is executed
	Then the delete result should be "Failure"
	And the recordset "[[rs().row]]" will be as follows
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |

Scenario: Delete the first record in a recordset 
	Given I have the following recordset
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |
	And I delete a record "[[rs(1)]]"
	When the delete tool is executed
	Then the delete result should be "Success"
	And the recordset "[[rs().row]]" will be as follows
	| rs       | val |
	| rs().row | 2   |
	| rs().row | 3   |	
	
Scenario: Delete a record using an index from a variable
	Given I have the following recordset
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |
	And an index "[[index]]" exists with a value "2"
	And I delete a record "[[rs([[index]])]]"
	When the delete tool is executed
	Then the delete result should be "Success"
	And the recordset "[[rs().row]]" will be as follows
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |

Scenario: Delete a record using a star notation
	Given I have the following recordset
	| rs       | val |
	| rs().row | 1   |
	| rs().row | 2   |
	| rs().row | 3   |
	And I delete a record "[[rs(*)]]"
	When the delete tool is executed
	Then the delete result should be "Success"
	And the recordset "[[rs().row]]" will be as follows
	| rs       | val |