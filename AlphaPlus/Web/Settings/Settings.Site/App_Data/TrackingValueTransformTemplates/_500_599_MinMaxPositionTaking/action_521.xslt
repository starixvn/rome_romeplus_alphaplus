<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:key name="pt-by-sporttype" match="pt" use="@sporttype" />
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc-ex">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
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
    <table class="tblDesc-ex">
      <xsl:for-each select="pt[count(.|key('pt-by-sporttype',@sporttype)[1])=1]">
        <tr class="headerSportTypeDetail">
          <td>
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
          </td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTMaster')" />
          </td>
          <td class="colW1">
          </td>
          <td class="colW2">
          </td>
        </tr>
        <xsl:for-each select="key('pt-by-sporttype', @sporttype)">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
            </td>
            <td  class="colW2">
              <xsl:value-of select="@masterminpt" />
            </td>
            <td class="colW1">
            </td>
            <td class="colW2">
            </td>
          </tr>
        </xsl:for-each>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>