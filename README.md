# SHARP RESOURCE EDITOR
A program that edit resources of .net project
Please note that this is the official repository of the program, that means official updates on source and releases will be commited/published here.

## Introduction
Sorry for my English, English is not my native language.

Sharp Resources Editor is tool that allows to edit .net project resources. The point is when your
project become localizable (more than one language) and need to be translated, it is easy to translate string resources, but the resources of windows forms may contains icons, images ...etc and it's hard to translate in normal way, the translator person then need to use Visual Studio to do the job and has experience with Forms design.

There is a tool ResGen.exe than MSDN mansion but it generate file by file, so if your project is huge you'll need to do a lot of work and waste a lot of time generating resources ...

To solve this problem, this program will do the hard work and leave the translation to you !

## System Requirements
- .net framework 4

## How to use ?
Before using, please note that: All forms and controls in your project have 'Localizable' set to true. 
See <https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/y99d1cd3(v=vs.100)?WT.mc_id=DT-MVP-5003235>

### If your project need to translated to NEW LANGUAGE:

- Run ResourcesGenerator.exe file first.
- A black screen should appear, then a folder browser should allow you to choose the source folder.
Browse for the project source folder (folder that contain all source folders including the solution file)
- Then a folder browser will appear again, browse where to save work space folder. This folder should be empty.
- Now a small dialog with one combobox will appear. Select the new language (translation language) of your project then click ok.
- Then the tool will start generating resources... after that, it will copy SharpResourcesEditor.exe to the workspace folder. SharpResourcesEditor.exe should run automatically and the resources with the new language should be listed in the left list.
- Select the first resource file from the left list. In the right, all string fields should appear, change the Translation field for each row to actual translation of the text.
- After finishing all translation for the file, click Save button or press "CTRL + S" to save.
After save success, the Translation fields should be equal to the Original Value rows with your translation.
- Complete all resources files in the left list the same way, translate them all to finish.
- Now the resources are ready, copy them then include them to your project or send it to the developer.

### IMPORTANT NOTE FOR DEVELOPERS: 
After copying the new generated resource files into your project, you'll need to include them one by one for each control and windows form using Visual Studio. Visual Studio will show these new resources 'grayed-out'.
Also, custom resources (not for controls nor windows forms) needs to be added manually. 

### If the project include the resources for a language but need to update them for new values:
In this case, you just need to edit the resources of your language:
- Copy SharpResourcesEditor.exe to the source folder or next to the source folder itself
- Run SharpResourcesEditor.exe, all resources should be listed in the left.
- In the tools strip, change the filter combobox to your language id.
- The left list should then updated to show resources of selected language.
- Select the first resource file from the left list. In the right, all string fields should appear, change the Translation field for each row to actual translation of the text.
- After finishing all translation for the file, click Save button or press "CTRL + S" to save.
After save success, the Translation fields should be equal to the Original Value rows with your translation.
- Complete all resources files in the left list the same way, translate them all to finish.
- Now the resources are ready, build your project or send the updated source to the developer.

## ResourcesGenerator Command lines:
- /copy: use copy mode, copy all resource files (with *.resx extension) into the target folder. No generation nor id chose.
- /noeditor: don't copy the 'Sharp Resource Editor' into the target folder.

## Sharp Resources Editor basics
In the main window of the program:

- Save: save changes for current selected resource file.
- Reload: reload the file (discard all changes)

- Filter by: the filter to use for resource files list.

All: show all files, regardless of id.

No ID: show only resource files that has no id. (i.g. Resource.resx)

xx-xx: show only resource files with id specified (i.g. Resource.en-US.resx for en-US id)

- Spell Check: to spell check translation for selected resource file.

Spell Check all: to spell check all rows for selected resource file.

Spell Check selected row: to spell check translation for selected row only for selected resource file.

Dictionary to use (important): to use spell check, you'll need dictionaries. In this menu, you must select the dictionary that you need to use in spell check process.

Download Dictionaries: to download more dictionaries for different languages. After download, the dictionaries will be listed in the 'Dictionary to use' menu and then you'll be able to use the desired one.


- Google Translate: to translate using google translate service.

Translate selected row: to translate selected row in selected resource file using google translate.

Translate all: to translate all rows in selected resource file using google translate.

- Folder: click this button to change the main resources (source) folder. Changing folder will reload the resource files list.

- Resource files list: here you can select the file you would like to translate/edit texts for.

- The table: with this table you'll be able to edit selected resource file from the resource files list. 
You'll only have to fill the texts in the Translation column. 
When done, use Save button (or press CTRL + S) to save changes.

## Notes:

- If a field of Translation is empty when opening the resource file at the first time, then there's not need to translate it and should be left empty.
- If a field is not understandable (e.g. toolstripbutton1, menustripitem14 ...etc) then leave it as it is.
- Don't worry about other resources like icons, images ..etc. SharpResourcesEditor.exe never modify these resource types and they kept in source itself when save.
- It's a good idea to take a look at the project using Visual Studio to see the translation (See "http://msdn.microsoft.com/en-us/library/y99d1cd3%28v=vs.80%29.aspx") you may need to fix some controls position and size after translation, because of the new language strings may be longer or shorter than it should.
  
### IMPORTANT NOTE FOR DEVELOPERS: 
After copying the new generated resource files into your project, you'll need to include them one by one for each control and windows form using Visual Studio. Visual Studio will show these new resources 'grayed-out'.
Also, custom resources (not for controls nor windows forms) needs to be added manually. If the resources are already included, no need to worry about anything, this applied only when generating new resources for new language.

