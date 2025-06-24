<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">
      <tr>
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
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('username')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@username"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('nickname')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@nickname"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('mobilephone')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@mphone"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('uplinename')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@uplinename"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ugroup')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@ugroup"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errcode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errmsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>