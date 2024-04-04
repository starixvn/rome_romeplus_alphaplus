<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
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
          <xsl:value-of select="xslLogFormat:GetLogResource('fax')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@fax"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('transferoption')" />
        </td>
        <td class="colW2" >
          <xsl:value-of select="xslLogFormat:TransferConditionFromCode(@transferoption)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('PositionTakingTemplate')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@templatename"/>
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('credit')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@credit"/>
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('maxcredit')"/>
        </td>
        <td class="colW2">
          <xsl:value-of select="@maxcredit"/>
        </td>
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
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('site')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@site" />
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