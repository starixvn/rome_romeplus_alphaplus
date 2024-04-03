<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:include href="../headerDetailBuilder.xslt"/>
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_custsetting_masterptupd">
    <table class="tblDesc-ex">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td  class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
      </tr>
    </table>
    <xsl:for-each select="pt">
      <div class="headerSportTypeDetail">
        <xsl:call-template name="buildDetailHeader" />
      </div>
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblPresetPTSuper')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@superpt" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isSuperAutoPT')" />
          </td>
          <td  class="colW2">
            <xsl:value-of select="@isautosuperpt" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblAutoPTSuper')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@autosuperpt" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTMaster')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@masterpt" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTMaster')" />
          </td>
          <td  class="colW2">
            <xsl:value-of select="@masterminpt" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('Updallmembers')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@updallmems" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('newspt')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@newsuperpt" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('IsLive')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@islive" />
          </td>
          <td class="colW1">
          </td>
          <td class="colW2">
          </td>
        </tr>
      </table>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>
