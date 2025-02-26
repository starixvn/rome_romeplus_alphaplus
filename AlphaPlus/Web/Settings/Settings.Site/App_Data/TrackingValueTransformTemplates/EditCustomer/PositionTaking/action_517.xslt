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
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
      </tr>
      <xsl:if test="pt">
        <tr style="text-align:center;" class="headerDetail">
          <td></td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('MasterPT')" />
          </td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('isMasterAutoPT')" />
          </td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('masterAutoPT')" />
          </td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTAgent')" />
          </td>
          <td style="text-align:center;">
            <xsl:value-of select="xslLogFormat:GetLogResource('MinPTAgent')" />
          </td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('Updallmembers')" />
          </td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('newmpt')" />
          </td>
        </tr>
        <xsl:for-each select="pt[count(.|key('pt-by-sporttype',@sporttype)[1])=1]">
          <tr class="headerSportTypeDetail">
            <td colspan="8">
              <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
            </td>
          </tr>
          <xsl:for-each select="key('pt-by-sporttype', @sporttype)">
            <tr>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@masterpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@isautomasterpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@automasterpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@agentpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@agentminpt" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@updallmems" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@newmasterpt" />
              </td>
            </tr>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>