<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
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
      <table  class="tblDesc">
        <tr>
          <xsl:choose>
            <xsl:when test="@sporttype = '206'">
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('Sport')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('discount')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@discount" />
              </td>
              <td class="colW1">
              </td>
              <td class="colW2">
              </td>
            </xsl:when>
            <xsl:otherwise>
              <xsl:if test="@sporttype">
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('Sport')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
                </td>
              </xsl:if>

              <xsl:if test="@bettype">
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('bettype')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
                </td>
              </xsl:if>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('discount')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@discount" />
              </td>
              <xsl:if test="not(@bettype)">
                <td class="colW1">
                </td>
                <td class="colW2">
                </td>
              </xsl:if>

              <xsl:if test="not(@sporttype)">
                <td class="colW1">
                </td>
                <td class="colW2">
                </td>
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>
        </tr>
      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
