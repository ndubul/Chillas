@Plugin
Feature: PluginSource
	In order to create plugins
	As a Warewolf User
	I want to be able to select dlls as a source to be used

Scenario: New Plugin Source File
	Given I open New Plugin Source
	And title is "New Plugin Source"
	And I click "File System"
	Then local drive "C:\" is visible in File window
	When I open "C:\" in File window
	Then GAC is not selected
	And Assembly is ""
	And "Save" is "Disabled"
	And "Cancel" is "Disabled"
	When I click 
	| Clicks                                   |
	| Development                              |
	| Dev                                      |
	| Binaries                                 |
	| MS Fakes                                 |
	| Microsoft QualityTools.Testing.Fakes.dll |
	Then "Save" is "Enabled"
	And Assembly is "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	When I change Assembly to "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dl"
	Then "Save" is "Disabled"
	When I change Assembly to "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	Then "Save" is "Enabled"
	When I save Plugin source
	Then Save Dialog is opened
	When I save as "Testing Resource Save"
	Then title is "Testing Resource Save"
	And "Save" is "Disabled"
	And "Cancel" is "Disabled"
	

Scenario: New Plugin Source GAC
	Given I open New Plugin Source
	When I click "GAC"
	Then the progress bar indicates loading of resources
	And Assembly is ""
	And "Save" is "Disabled"
	When I Filter for "AuditPolicyGPMan"
	And I click "AuditPolicyGPManagedStubs.Interop, Version=6.1.0.0"
	Then Assembly is "GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	And "Save" is "Enabled"
	When I save Plugin source
	Then Save Dialog is opened
	 
#I cant get to these paths so am leaving them as per Murali as I cant get to this edit page either.
#Assembly will need to change.
Scenario: Editing Plugin Source
	Given I open "Test" plugin source
	Then title is "Edit Plugin Source - Test"
	And file is selected
	And "C:\" is "visible"
	And Assembly is "Plugins/Unlimited.Email.Plugin.dll"
	And "Save" is "Disabled"
	When I select "PrimativesTestDLL - Copy.dll"
	Then Assembly is "Z:\Plugins\PrimativesTestDLL - Copy.dll"
	And "Save" is "Enabled"
	When I save Plugin source
	Then Save Dialog is opened

Scenario: Change Plugin Source Assembly Input
	Given I open "Test" plugin source
	Then title is "Edit Plugin Source - Test"
	And file is selected
	And "C:\" is "visible"
	And Assembly is "Plugins/Unlimited.Email.Plugin.dll"
	And "Save" is "Disabled"
	When I select "PrimativesTestDLL - Copy.dll"
	Then Assembly is "Z:\Plugins\PrimativesTestDLL - Copy.dll"
	And "Save" is "Enabled"
	When I save Plugin source
	Then Save Dialog is opened

Scenario: Refresh New Plugin Source File
	Given I open New Plugin Source
	When I click 
	| Clicks                                   |
	| File                                     |
	| C:\                                      |
	| Development                              |
	| Dev                                      |
	| Binaries                                 |
	| MS Fakes                                 |
	| Microsoft QualityTools.Testing.Fakes.dll |
	Then "Save" is "Enabled"
	When I refresh the filter
	Then File is reloaded
	And Assembly is "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	And "MS Fakes" is "Visible"
	And "Microsoft QualityTools.Testing.Fakes.dll" is selected

Scenario: Refresh New Plugin Source GAC
	Given I open New Plugin Source
	When I click "GAC"
	And I click "GAC"
	Then GAC is not loading
	And GAC tree is collapsed
	When I click "GAC"
	When I filter for "playback"
	Then the progress bar indicates loading of resources
	And "Microsoft.MediaCenter.Playback" is "visible"
	And GAC only has one option in the tree
	When I refresh the filter
	And GAC only has one option in the tree
	