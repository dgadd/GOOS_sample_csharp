README.txt for GOOS sample code in Visual Studio 2010
================================================

David Gadd
http://www.twitter.com/gaddzeit
Email: gaddzeit@yahoo.ca

This version of the sample code from #goos was written using:
* Visual Studio 2010 Beta 2.0
* Visual Studio's unit-testing framework 
(I experienced NUnit compatability issues in VS 2010 Beta 2.0;
to use this code in Visual Studio 2008 with NUnit just retag the test class/method attributes.)
* Reshaper Beta5 (helpful but not required to run code)
* Rhino Mocks version 3.6.0.0 (assume this reference will be broken; you will need to re-reference to a local copy)

This version is complete as of the end of Chapter 14, with all acceptance and unit tests passing.

While I used OpenServer for the Java version, for this version I created a fake XMPP server (all method calls are similar/identical).

Instead of using WindowLicker for a full end-to-end acceptance test, I am simply mocking the IPickerMainView interface
and verifying in RhinoMocks that the SniperStatus string property is being set. To achieve this, the level of scope in
the end-to-end acceptance test is not identical to the Java code; above the method calls to ApplicationRunner.cs
and FakeAuctionServer.cs I am setting RhinoMocks expectations on _mockPickerMainView.

Other than that, I am using the more commonly-used conventions in C# than in Java of:
* prefixing interface names with capital I
* underscore prefixing instance variables

If you have any questions feel free to tweet or email me.

David Gadd