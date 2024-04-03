<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:include href="../../headerDetailBuilder.xslt"/>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_admin_disablenewproductstatusupd">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>
    </table>

    <xsl:for-each select="productstatus">
      <div class="headerSportTypeDetail">
        <xsl:call-template name="buildDetailHeader" />
      </div>
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isdisabled')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isdisabled" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('updateAllMasters')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@updateallmasters" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isDisabledAllMasters')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@disableallmasters" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('updateAllAgents')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@updateallagents" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isDisabledAllAgents')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@disableallagents" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('updateAllMembers')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@updateallmembers" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isDisabledAllMembers')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@disableallmembers" />
          </td>
          <td class="colW1"></td>
          <td class="colW2"></td>
          <td class="colW1"></td>
          <td class="colW2"></td>
        </tr>
      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>