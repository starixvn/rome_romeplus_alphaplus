<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:key name="groupid" match="winlimit" use="@groupid" />
  <xsl:template match="root">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
      </tr>
      </table>
      <table class="tblDesc">
      <tr class="headerDetail" style="text-align:center;">
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('MaximumWin')" />
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('DailyAutoReset')" />
        </td>
      </tr>
      <xsl:for-each select="winlimit[count(.|key('groupid',@groupid)[1])=1]">
        <tr class="headerSportTypeDetail">
          <td colspan="3">
            <xsl:value-of select="xslLogFormat:GetProductGroupName(@groupid)" />
          </td>
        </tr>
        <xsl:for-each select="key('groupid', @groupid)">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetDisplayName(@settinglevel,@sporttype,@bettype,@productid)" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@maxwinlimit" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@isautoreset" />
            </td>
          </tr>
        </xsl:for-each>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>