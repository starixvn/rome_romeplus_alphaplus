<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_bonus_confirmpayment">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
        </td>
        <td class="colW2"  >
          <xsl:value-of select="@subacc" />
        </td>

        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('BonusRefNo')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@refno" />
        </td>

        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('PaymentDate')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@paymentdate" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
