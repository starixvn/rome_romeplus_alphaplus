<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:key name="groupid" match="betlimit" use="@groupid" />
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
          <xsl:value-of select="xslLogFormat:GetLogResource('sourceaccount')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@sourceaccount" />
        </td>
        <td class="colW1">
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
        <td class="colW1">
        </td>
      </tr>
      <tr class="headerSportTypeDetail" style="text-align:center;">
        <td>
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" />
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('MaxBet')" />
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('MaxPerMatch')" />
        </td>
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('MaxPerBall')" />
        </td>
      </tr>
      <xsl:for-each select="betlimit[count(.|key('groupid',@groupid)[1])=1]">
        <tr class="headerSportTypeDetail">
          <td colspan="5">
            <xsl:value-of select="xslLogFormat:GetProductGroupName(@groupid)" />
          </td>
        </tr>
        <xsl:for-each select="key('groupid', @groupid)">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@minbet" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@maxbet" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@maxpermatch" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@maxpernumber" />
            </td>
          </tr>
        </xsl:for-each>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>