; example1.nsi
;
; This script is perhaps one of the simplest NSIs you can make. All of the
; optional settings are left to their default settings. The installer simply 
; prompts the user asking them where to install, and drops a copy of example1.nsi
; there. 
;
; example2.nsi expands on this by adding a uninstaller and start menu shortcuts.

;--------------------------------

!define APP "OutlookIMExToolsAddIn1"
!define BINDIR "bin\Debug"

!system 'MySign "${BINDIR}\${APP}.dll"'
!finalize 'MySign "%1"'

!system 'DefineAsmVer.exe "${BINDIR}\${APP}.dll" "!define VER ""[ProductVersion]"" " "!define F4VER ""[F4VER]"" " "!define LegalCopyright ""[LegalCopyright]"" " "!define FileDescription ""[FileDescription]"" " "!define FileVersion ""[FileVersion]"" " > Appver.tmp'
!include "Appver.tmp"
!searchreplace APV "${VER}" "." "_"

; <IncludeSourceRevisionInInformationalVersion>false</IncludeSourceRevisionInInformationalVersion>

VIProductVersion "${F4VER}"
VIAddVersionKey FileDescription "${FileDescription}"
VIAddVersionKey FileVersion "${FileVersion}"
VIAddVersionKey LegalCopyright "${LegalCopyright}"
VIAddVersionKey ProductVersion "${VER}"

; The name of the installer
Name "${APP}"

; The file to write
OutFile "Setup_${APP}.exe"

; Request application privileges for Windows Vista
RequestExecutionLevel user

; Build Unicode installer
Unicode True

; The default installation directory
InstallDir "$APPDATA\${APP}"

XPStyle on

;--------------------------------

; Pages

Page directory
Page components
Page instfiles

;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File /r "${BINDIR}\*"
  
SectionEnd

Section "Launch VSTO file"
  ExecShellWait "open" 'OutlookIMExToolsAddIn1.vsto'
SectionEnd
