<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_custsetting_stakelevelupd">
    <xsl:param name="productname" select="xslLogFormat:GetProductName(@productid)"></xsl:param>
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
    <xsl:for-each select="stakelevel">
      <xsl:choose>
        <xsl:when test="@bettype">
          <div class="headerSportTypeDetail">
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
          </div>
        </xsl:when>
        <xsl:otherwise>
          <div class="headerSportTypeDetail">
            <xsl:value-of select = "$productname" />
          </div>
        </xsl:otherwise>
      </xsl:choose>
      <table  class="tblDesc">
        <tr>
          <xsl:choose>
            <xsl:when test="@bettype">
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </td>
            </xsl:when>
            <xsl:when test="@sporttype">
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
              </td>
            </xsl:when>
            <xsl:when test="@productid">
              <td class="colW1">
                <xsl:value-of select="$productname" />
              </td>
            </xsl:when>
            <xsl:otherwise>
              <td class="colW1">
              </td>
            </xsl:otherwise>
          </xsl:choose>
          <td class="colW2">
            <xsl:value-of select="@stakelevellist" />
          </td>
          <td class="colW1">
          </td>
          <td class="colW2">
          </td>
          <td class="colW1">
          </td>
          <td  class="colW2">
          </td>
        </tr>
      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>