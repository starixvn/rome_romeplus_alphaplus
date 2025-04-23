<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_custsetting_disablegamestatusupd">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
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
    
    <xsl:for-each select="productstatus">
      <div class="headerSportTypeDetail">
        <xsl:choose>
          <xsl:when test="@productid">
            <xsl:choose>
              <xsl:when test="@bettype">
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" /> -
                <xsl:value-of select="xslLogFormat:GetBettypeName(@bettype)" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
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
            <xsl:value-of select="xslLogFormat:GetLogResource('isdisabled')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isdisabled" />
          </td>
          
          <xsl:choose>
            <xsl:when test="@isdisableddownline">
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('isdisableddownline')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@isdisableddownline" />
              </td>
            </xsl:when>
            <xsl:otherwise>
              <td class="colW1">
              </td>
              <td class="colW2">
              </td>
            </xsl:otherwise>
          </xsl:choose>
          
          <td class="colW1">
          </td>
          <td class="colW2">
          </td>
        </tr>
      </table>
    </xsl:for-each>        
  </xsl:template>
</xsl:stylesheet>
