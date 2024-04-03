<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:key name="pt-by-sporttype" match="pt/r" use="@sporttype" />
  <xsl:template match="root">
    <table class="tblDesc">
      <xsl:if test="pt/r">
        <tr style="text-align:center;" class="headerDetail">
          <td></td>
          <td style="text-align:center;" colspan="5">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblAgentPT')" />
          </td>
        </tr>
        <xsl:for-each select="pt/r[count(.|key('pt-by-sporttype',@sporttype)[1])=1]">
          <tr class="headerSportTypeDetail">
            <td colspan="6">
              <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
            </td>
          </tr>
          <xsl:for-each select="key('pt-by-sporttype', @sporttype)">
            <tr>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </td>
              <td class="colW2" colspan="5">
                <xsl:value-of select="@agentpt" />
              </td>
            </tr>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>