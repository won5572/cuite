﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using CUITe.Controls;
using CUITe.Controls.HtmlControls;
using CUITe.Controls.SilverlightControls;
using Sample_CUITeTestProject.ObjectRepository;
using Microsoft.VisualStudio.TestTools.UITest.Extension;

namespace Sample_CUITeTestProject
{
    [CodedUITest]
    [DeploymentItem(@"Sample_CUITeTestProject\XMLFile2.xml")]
    [DeploymentItem(@"Sample_CUITeTestProject\TestHtmlPage.html")]
    public class Tests_for_HtmlControls
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //CUITe_BrowserWindow.CloseAllBrowsers();
        }

        [TestMethod]
        public void SampleTestNumber1()
        {
            GoogleHomePage pgGHomePage = CUITe_BrowserWindow.Launch<GoogleHomePage>("http://www.google.com");
            pgGHomePage.txtSearch.SetText("Coded UI Test Framework");
            GoogleSearch pgSearch = CUITe_BrowserWindow.GetBrowserWindow<GoogleSearch>();
            UITestControlCollection col = pgSearch.divSearchResults.UnWrap().GetChildren();
            //do something with collection            
            pgSearch.Close();
        }

        [TestMethod]
        public void Test_DataManager_EmbeddedResource()
        {
            Hashtable ht = CUITe.CUITe_DataManager.GetDataRow(Type.GetType("Sample_CUITeTestProject.Tests_for_HtmlControls"), "XMLFile1.xml", "tc2");
            Assert.AreEqual("test", ht["test"]);
            Assert.AreEqual("Kondapur, Hyderabad", ht["address"]);
            Assert.AreEqual("Suresh", ht["firstname"]);
            Assert.AreEqual("Balasubramanian", ht["lastname"]);
            Assert.AreEqual("04/19/1973", ht["dob"]);
            Assert.AreEqual("37", ht["age"]);
            Assert.AreEqual("Indian", ht["nationality"]);
        }

        //[TestMethod]
        //public void Test_DataManager_DeploymentItem()
        //{
        //    Hashtable ht = CUITe_DataManager.GetDataRow("XMLFile2.xml", "content2");
        //    Assert.AreEqual("SomeTest", ht["test"]);
        //    Assert.AreEqual("Somewhere, Somewhere", ht["address"]);
        //    Assert.AreEqual("SomeFirstName", ht["firstname"]);
        //    Assert.AreEqual("SomeLastNameBigger", ht["lastname"]);
        //    Assert.AreEqual("01/01/1900", ht["dob"]);
        //    Assert.AreEqual("101", ht["age"]);
        //    Assert.AreEqual("USA", ht["nationality"]);
        //}

        [TestMethod]
        public void Telerik_Combo()
        {
            ASPNETComboBoxDemoFirstLook pgPage = CUITe_BrowserWindow.Launch<ASPNETComboBoxDemoFirstLook>(
                "http://demos.telerik.com/aspnet-ajax/combobox/examples/default/defaultcs.aspx");
            Thread.Sleep(5000);
            pgPage.Refresh();
            Thread.Sleep(5000);
            pgPage.combo1.SelectItemByText("Tofu", 5000);
            pgPage.combo2.SelectItemByText("Bloomfield Hills", 5000);
            pgPage.combo3.SelectItemByText("Exotic Liquids", 5000);
            pgPage.combo4.SelectItemByText("American Express", 5000);
            pgPage.Close();
        }

        [TestMethod]
        public void Test_FeatureRequest_608()
        {
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch("http://mail.google.com", "Gmail: Email from Google");
            bWin.Get<CUITe_HtmlEdit>("Id=Email").SetText("xyz@gmail.com");
            bWin.Get<CUITe_HtmlPassword>("Id=Password").SetText("MyPa$$Word");
            bWin.Get<CUITe_HtmlInputButton>("Id=signIn").Click();
            bWin.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(CUITe.CUITe_InvalidSearchKey))]
        public void Test_FeatureRequest_588()
        {
            Google pgGHome = CUITe_BrowserWindow.Launch<Google>("http://www.google.com");
            pgGHome.div588.Click();
            pgGHome.Close();
        }

        [TestMethod]
        public void Test_InvalidControlExists()
        {
            Playback.PlaybackSettings.SmartMatchOptions = SmartMatchOptions.None; //required if you are using .Exists on an invalid control
            CUITe_BrowserWindow.Launch("http://www.google.com");
            GoogleHomePage pgGHomePage = CUITe_BrowserWindow.GetBrowserWindow<GoogleHomePage>();
            Assert.IsFalse(pgGHomePage.divInvalid.Exists);
        }

        [TestMethod]
        public void Test_FeatureRequest_589()
        {
            GoogleHomePage pgGHomePage = CUITe_BrowserWindow.Launch<GoogleHomePage>("http://www.google.com");
            
            HtmlEdit tmp = new HtmlEdit(pgGHomePage);
            tmp.SearchProperties.Add("Id", "lst-ib");

            CUITe_HtmlEdit txtEdit = new CUITe_HtmlEdit();
            txtEdit.WrapReady(tmp);
            txtEdit.SetText("Coded UI Test enhanced Framework");
            GoogleSearch pgSearch = CUITe_BrowserWindow.GetBrowserWindow<GoogleSearch>();
            pgSearch.Close();
        }

        [TestMethod]
        public void Test_HtmlTable_GetColumnHeaders()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html");
            CUITe_BrowserWindow bWin = new CUITe_BrowserWindow("A Test");
            CUITe_HtmlTable tbl = bWin.Get<CUITe_HtmlTable>("id=calcWithHeaders");
            string[] saExpectedValues = new string[] { "Header1", "Header2", "Header3" };
            string[] saHeaders = tbl.GetColumnHeaders();
            Assert.AreEqual(saExpectedValues[0], saHeaders[0]);
            Assert.AreEqual(saExpectedValues[1], saHeaders[1]);
            Assert.AreEqual(saExpectedValues[2], saHeaders[2]);
            bWin.Close();
        }

        [TestMethod]
        public void Test_HtmlTableIssue_638_WithHeaders()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            CUITe_HtmlTable tbl = bWin.Get<CUITe_HtmlTable>("id=calcWithHeaders");
            tbl.FindRowAndClick(2, "9", CUITe_HtmlTableSearchOptions.NormalTight);
            Assert.IsTrue(tbl.GetCellValue(2,2).Trim() == "9");
            bWin.Close();
        }

        [TestMethod]
        public void Test_HtmlTableIssue_638_WithOutHeaders()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            CUITe_HtmlTable tbl = bWin.Get<CUITe_HtmlTable>("id=calcWithOutHeaders");
            tbl.FindRowAndClick(2, "9", CUITe_HtmlTableSearchOptions.NormalTight);
            Assert.IsTrue(tbl.GetCellValue(2, 2).Trim() == "9");
            bWin.Close();
        }

        [TestMethod]
        public void Test_HtmlTableIssue_TH_inTBODY()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html");
            CUITe_BrowserWindow bWin = new CUITe_BrowserWindow("A Test");
            CUITe_HtmlTable tbl = bWin.Get<CUITe_HtmlTable>("id=TabContainer1_TabPanel1_gvSourceLuns");
            tbl.FindRowAndClick(0, "LUN_04", CUITe_HtmlTableSearchOptions.NormalTight);
            Assert.IsTrue(tbl.GetCellValue(0, 0).Trim() == "LUN_04");
            bWin.Close();
        }

        [TestMethod]
        public void Test_Value_As_SearchParameterKey()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            bWin.Get<CUITe_HtmlInputButton>("Value=Log In").Click();
            bWin.Close();
        }

        [TestMethod]
        public void Test_FileInput()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            bWin.Get<CUITe_HtmlFileInput>("Id=ctl00_PlaceHolderMain_ctl01_ctl02_InputFile").SetFile(@"C:\Demo\info.txt");
            bWin.Close();
        }

        [TestMethod]
        public void Test_SharePoint2010()
        {
            CUITe_BrowserWindow.Launch("http://myasia/sites/sureba/Default.aspx");
            CUITe_BrowserWindow.Authenticate("username", "passwd");
            CUITe_BrowserWindow bWin = new CUITe_BrowserWindow("Suresh Balasubramanian");
            bWin.Get<CUITe_HtmlHyperlink>("Id=idHomePageNewDocument").Click();
            var closeLink = bWin.Get<CUITe_HtmlHyperlink>("Title=Close;class=ms-dlgCloseBtn");
            //clicking closeLink directly doesn't work as the maximizeLink is clicked due to the controls being placed too close to each other
            Mouse.Click(closeLink.UnWrap().GetChildren()[0].GetChildren()[0]); 
            bWin.RunScript(@"STSNavigate2(event,'/sites/sureba/_layouts/SignOut.aspx');");
        }

        [TestMethod]
        public void Test_HtmlGetChildren()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            var div = bWin.Get<CUITe_HtmlDiv>("id=calculatorContainer1");
            var col = div.GetChildren();
            Assert.IsTrue(col[0].GetBaseType().Name == "HtmlDiv");
            Assert.IsTrue(col[1].GetBaseType().Name == "HtmlTable");
            Assert.IsTrue(((CUITe_HtmlDiv)col[0]).InnerText == "calcWithHeaders");
            var tbl = (CUITe_HtmlTable)col[1];
            Assert.IsTrue(tbl.GetCellValue(2, 2).Trim() == "9");
            bWin.Close();
        }

        [TestMethod]
        public void Test_CUITe_HtmlParagraph()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            Assert.IsTrue(bWin.Get<CUITe_HtmlParagraph>("Id=para1").InnerText.Contains("CUITe_HtmlParagraph"));
            bWin.Close();
        }

        [TestMethod]
        public void Test_CUITe_HtmlComboBox_Items()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            var cmb = bWin.Get<CUITe_HtmlComboBox>("Id=select1");
            Assert.AreEqual("Football", cmb.Items[1]);
            Assert.IsTrue(cmb.ItemExists("Cricket"));
            bWin.Close();
        }

        [TestMethod]
        public void Test_CUITe_HtmlParagraph_objrep()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            TestHtmlPage testpage = CUITe_BrowserWindow.Launch<TestHtmlPage>(baseDir + "/TestHtmlPage.html");
            string content = testpage.p.InnerText;
            Assert.IsTrue(content.Contains("CUITe_HtmlParagraph"));
            testpage.Close();
        }

        [TestMethod]
        public void Test_Traversals()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/TestHtmlPage.html", "A Test");
            var p = bWin.Get<CUITe_HtmlParagraph>("Id=para1");
            Assert.IsTrue(((CUITe_HtmlEdit)p.PreviousSibling).UnWrap().Name == "text1_test");
            Assert.IsTrue(((CUITe_HtmlInputButton)p.NextSibling).ValueAttribute == "sample button");
            Assert.IsTrue(((CUITe_HtmlDiv)p.Parent).UnWrap().Id == "parentdiv");
            Assert.IsTrue(((CUITe_HtmlPassword)p.Parent.FirstChild).UnWrap().Name == "pass");
            bWin.Close();
        }

        [TestMethod]
        [DeploymentItem(@"Sample_CUITeTestProject\iframe_test.html")]
        [DeploymentItem(@"Sample_CUITeTestProject\iframe.html")]
        public void Html_ClickButtonInIFrame()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
            CUITe_BrowserWindow bWin = CUITe_BrowserWindow.Launch(baseDir + "/iframe_test.html", "iframe Test Main");
            bWin.Get<CUITe_HtmlInputButton>("Value=Log In").Click();
            bWin.Close();
        }
    }
}

