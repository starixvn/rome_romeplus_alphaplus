<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:include href="../headerDetailBuilder.xslt"/>
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_custsetting_winlosslimitupd">
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
    <xsl:for-each select="losslimit">
      <div class="headerSportTypeDetail">
        <xsl:call-template name="buildDetailHeader" />
      </div>
      <table class="tblDesc-ex">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MaximumLoss')" />
          </td>
          <td  class="colW2">
            <xsl:value-of select="@maxlosslimit" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('DailyAutoReset')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isautoreset" />
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