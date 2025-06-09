<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="r">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('username')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@username"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('firstname')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@fname"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lastname')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@lname"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('phone')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@phone"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('mobilephone')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@mphone"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('bank')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@bankid"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('accnumber')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@accnumber"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('recommend')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@recommend"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ugroup')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@ugroup" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
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
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Platform')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetPlatformOfSearchAndCopyFunction(@systemid)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errcode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errmsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>