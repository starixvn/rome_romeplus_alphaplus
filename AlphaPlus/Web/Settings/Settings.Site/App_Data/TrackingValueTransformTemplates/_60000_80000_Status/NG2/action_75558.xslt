<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_admin_disablegamestatusupd">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>      
    </table>
    
    <xsl:for-each select="productstatus">
      <div class="headerSportTypeDetail">
        <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
      </div>
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isdisabled')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isdisabled" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('updateAllMembers')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@updateallmembers" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isDisabledAllMembers')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@disableallmembers" />
          </td>
        </tr>
      </table>
    </xsl:for-each>        
  </xsl:template>
</xsl:stylesheet>
