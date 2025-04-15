<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:key name="pt-by-sporttype" match="pt" use="@sporttype" />
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
      <xsl:if test="pt">
        <tr style="text-align:center;" class="headerDetail">
          <td></td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('SuperPT')" />
          </td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('isSuperAutoPT')" />
          </td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('lblAutoPTSuper')" />
          </td>
        </tr>
        <xsl:for-each select="pt[count(.|key('pt-by-sporttype',@sporttype)[1])=1]">
          <tr class="headerSportTypeDetail">
            <td colspan="4">
              <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
            </td>
          </tr>
          <xsl:for-each select="key('pt-by-sporttype', @sporttype)">
            <tr>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@superpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@isautosuperpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@autosuperpt" />
              </td>
            </tr>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>