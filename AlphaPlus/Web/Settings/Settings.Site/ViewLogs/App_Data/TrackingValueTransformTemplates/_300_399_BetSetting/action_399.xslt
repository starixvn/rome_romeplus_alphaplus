<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="r">

    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('custid')" />
        </td>
        <td  class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
        </td>
        <td  class="colW2">
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>

      </tr>
    </table>
    <xsl:for-each select="betlimit">
      <div class="headerSportTypeDetail">
        <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
      </div>
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" />
          </td>
          <td  class="colW2">
            <xsl:value-of select="@minbet" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MaxBet')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@maxbet" />
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