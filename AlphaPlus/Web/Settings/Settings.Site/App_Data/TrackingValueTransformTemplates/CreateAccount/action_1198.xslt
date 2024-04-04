<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="root">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('isinternal')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@isinternal" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('suspended')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@suspended" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('closed')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@closed" />
        </td>
      </tr>


      <xsl:choose>
        <xsl:when test="@disablenumber">
          <!-- Old log before migrate Number Game & RNG Keno to DCS-->
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('doublecommission')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@doublecommlosing" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('DisabledESports')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disable43_1" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('DisableNumber')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disablenumber" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('DisableOutright')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disableoutright" />
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
          <xsl:if test="xslLogFormat:IsVisibleKeno(@hidekeno) = 'True'">
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1501')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1501" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1515')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1515" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1516')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1516" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1517')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1517" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1519')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1519" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1520')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1520" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1521')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1521" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1522')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1522" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1524')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1524" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1525')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1525" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1526')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1526" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1527')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1527" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1528')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1528" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1529')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1529" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1530')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1530" />
              </td>
            </tr>

            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisabledKenoBetTypeName('1531')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable202_1531" />
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

          </xsl:if>
          <xsl:for-each select="@*[name() != 'disable202_1501']">
            <xsl:value-of select="xslLogFormat:RenderCustStatus(name(), .)"/>
          </xsl:for-each>
        </xsl:when>
        <xsl:otherwise>
          <!-- New log -->
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('doublecommission')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@doublecommlosing" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('DisabledESports')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disable43_1" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('DisableOutright')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disableoutright" />
            </td>
          </tr>
          <xsl:for-each select="@*">
            <xsl:value-of select="xslLogFormat:RenderCustStatus(name(), .)"/>
          </xsl:for-each>
        </xsl:otherwise>
      </xsl:choose>
    </table>
  </xsl:template>
</xsl:stylesheet>
