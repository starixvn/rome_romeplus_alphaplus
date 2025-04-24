<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_custsetting_discountupd">
    <table class="tblDesc">
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
    <xsl:for-each select="discount">
      <div class="headerSportTypeDetail">
        <xsl:choose>
          <xsl:when test="@sporttype">
            <xsl:choose>
              <xsl:when test="@bettype">
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" /> -
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
          </xsl:otherwise>
        </xsl:choose>
      </div>
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v204discountvalue_commission')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@discount" />
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
