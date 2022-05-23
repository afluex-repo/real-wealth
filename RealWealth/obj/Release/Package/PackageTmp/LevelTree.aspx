<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LevelTree.aspx.cs" Inherits="MyTrade.LevelTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="hdf">
    <!-- Required meta tags -->
   
    <title>MyTrade</title>
</head><body>
    <form id="form1" runat="server">
        <div id="BrokerTree">
            <asp:TreeView ID="trvBroker" runat="server" ExpandDepth="1" ImageSet="XPFileExplorer" ForeColor="Red" Width="327px" BorderColor="#993366" NodeIndent="15">
                <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                <LeafNodeStyle ForeColor="Red" ImageUrl="~/TreeIcon/men-inactive.png" NodeSpacing="5px" />
                <NodeStyle CssClass="gridViewToolTip" Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                <ParentNodeStyle Font-Bold="False" ImageUrl="~/TreeIcon/men-active.png" NodeSpacing="5px" Width="200px" ForeColor="Orange" />
                <RootNodeStyle BackColor="Orange" Font-Bold="True" Font-Names="Times New Roman" ForeColor="White" NodeSpacing="5px" Width="200px" />
                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </div>
    </form>
</body>
</html>
