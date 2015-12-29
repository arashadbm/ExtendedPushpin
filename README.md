# ExtendedPushpin
Extended Pushpin with a floating Tool Tip when user taps on it.

###Features
- A floating Tooltip which is shown when user the taps on pushpin.
- Toolip uses popup instead of controlling visibility of hidden grid, 
 which prevents flickering, reposition and other icons hiding part of tooltip. 
- Open Animation is implemented in Xaml, so you can play with it using Styling.
- Changing default blue icon using ContentTemplate of Pushpin.

###Screenshots
![UWP screenshot](https://raw.githubusercontent.com/arashadm/ExtendedPushpin/blob/master/Images/Pushpin_UWP.JPG "UWP screenshot")

![Windows phone 8.0 screenshot](https://raw.githubusercontent.com/arashadm/ExtendedPushpin/blob/master/Images/Pushpin_WP80.png "Windows phone 8.0 screenshot")

###Status
Alpha, Still needs some improvements. Check Todo section.

###Target platforms
- Windows Phone 8.X Silverlight will use ExtendedPushpin.WPSL80 class library.
- Windows phone 8.1 RT and Windows 8.1 will use ExtendedPushpin.WINRT81 portable class library.
- Universal Windows Application (UWP-10) will use ExtendedPushpin.UWP class library.

Currently the project doesn't have Nuget, you will need to pull the source code and copy paste the library that targets your application.

I will add Nuget packages later when I finish the improvements in Todo section

###Samples
You can find samples for the following platforms:
- Windows phone 8.0 Silver Light (Sample.WPSL80) works on WPSL8.1 also.
- Windows phone 8.1 RT. (Sample.WP81)
- Universal Windows Application 10 (Sample.UWP)

The samples show sample of pushpins of Berlin city,
I have made them as simple as showing the pushpin using codebehind but ofcourse you can use them easily with binding to collections in viewmodel.

###Issues
- Tooltip is shown over other controls in Page when map isn't taking the whole page.
Please report any issue you find or contact if you have enhancements, fixes or ideas.

###Todo
- Improve offest calculation mechanism used to show the tooltip above the pushpin, check ExpandedPanelLayoutUpdated method.
- Fix Tooltip is shown over other controls in Page when map isn't taking the whole page.
- Test different templates in both control template and expansion template to make sure it acts correctly.
- Add Nuget Packages.

###License
GNU v3, check license file under main repositry.


