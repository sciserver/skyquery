<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Basic/UI.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SkyQuery.Web.UI.Default" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        .carousel-control.left, .carousel-control.right {
            background-image: none;
        }

        .carousel-inner .item {
            background: #FFFFFF;
            width: 100%;
            height: 480px;
        }

            .carousel-inner .item div {
                background: #F0F0F0;
                background-size: contain;
                width: 960px;
                margin-left: auto;
                margin-right: auto;
                height: 480px;
                padding-left: 280px;
                padding-top: 12px;
                color: #FFFFFF;
            }

                .carousel-inner .item div p {
                    font-size: 18px;
                    font-weight: bold;
                }

        .carousel-indicators {
            bottom: -50px;
        }

            .carousel-indicators li {
                background-color: #FFFFFF;
                border-radius: 0;
                border: 1px solid #C0C0C0;
            }


            .carousel-indicators .active {
                background-color: #C0C0C0;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <%--<jgwuc:Form runat="server" ID="WelcomeForm" ImageUrl="~/Images/skyquery.png">
        <FormTemplate>
            <ul>
                <li>Access a growing list of astronomical catalogs</li>
                <li>Access your own data in SkyServer MyDB</li>
                <li>Write SQL to filter and process data</li>
                <li>Do efficient spatial filtering</li>
                <li>Run full-catalog cross-matches</li>
                <li>Cross-match multiple catalogs using an algorithm based on parallel data processing and Bayesian statistics</li>
                <li>Upload your data and download cross-match results in various standard formats</li>
            </ul>
            <p>
                Since SkyQuery provides high performance data processing with storage
                allocated for each user, you have to sign in with you existing SciServer (former SkyServer)
                account. Click on the sign-in link in the top right corner to start.
            </p>
        </FormTemplate>
        <ButtonsTemplate>
        </ButtonsTemplate>
    </jgwuc:Form>--%>

    <div id="myCarousel" class="carousel slide dock-hfill" data-ride="carousel">
        <!-- Indicators -->
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
            <!--<li data-target="#myCarousel" data-slide-to="3"></li>-->
        </ol>

        <!-- Wrapper for slides -->
        <div class="carousel-inner">
            <div class="item active">
                <div style="background-image: url('Assets/Home/home_1.jpg')">
                    <h1>Welcome to SkyQuery</h1>
                    <p>
                        SkyQuery is a scalable database system for cross-matching astronomical source catalogs
                    </p>
                    <ul>
                        <li>Cross-match multiple catalogs</li>
                        <li>Access a growing list of surveys</li>
                        <li>Access your own data in SkyServer MyDB</li>
                        <li>Write SQL to filter and process data</li>
                        <li>Do efficient spatial filtering</li>
                        <li>Run full-catalog cross-matches</li>
                    </ul>
                </div>
            </div>
            <div class="item">
                <div style="background-image: url('Assets/Home/home_2.jpg')">
                    <h1>Integrated with SciServer</h1>
                    <p>SkyQuery is integrated with SciServer, the collaborative data analysis platform
                        for big data
                    </p>
                    <ul>
                        <li>MyDB shared with SkyServer CasJobs</li>
                        <li>Access terabytes of scratch space from MyScratch DB</li>
                        <li>Access your files in SciDrive</li>
                        <li>Run your python scripts from SciServer Compute</li>
                        <li>Use the RESTful API to integrate with you scripts</li>
                    </ul>
                </div>
            </div>
            <div class="item">
                <div style="background-image: url('Assets/Home/home_3.jpg')">
                    <h1>High-performance, scalable platform</h1>
                    <p>SkyQuery runs on a cluster of state-of-the art, all-solid state servers</p>
                    <ul>
                        <li>Scalability allows full catalog cross-matching in reasonable time</li>
                        <li>All solid-state background storage for fast data access</li>
                        <li>NVMe drives for temporary storage</li>
                        <li>Cluster setup offers parallel execution and high availablity</li>
                        <li>Cross match jobs are written in extended SQL</li>
                    </ul>
                </div>
            </div>
            <!--<div class="item">
                <div>
                    <h1>Access your own data in SkyServer MyDB</h1>
                </div>
            </div>-->
        </div>

        <!-- Left and right controls -->
        <a class="left carousel-control" href="#myCarousel" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left"></span>
            <span class="sr-only">Previous</span>
        </a>
        <a class="right carousel-control" href="#myCarousel" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right"></span>
            <span class="sr-only">Next</span>
        </a>
    </div>

</asp:Content>
