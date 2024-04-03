<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmastergolddeluxe_positiontaking">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblPresetPTSuper')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@v255spositiontaking_1701" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblAutoPTSuper')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@auto_v255spositiontaking_1701" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('isSuperAutoPT')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@isauto_v255spositiontaking_1701" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTMaster')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@v255mpositiontaking_1701" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Updallmembers')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@updateallmembers" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('newspt')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@newspositiontaking" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1"></td><td class="colW2"></td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
