include ..\..\..\MakeVars

build: build-cls build-client


build-cls:
	if not exist bin mkdir bin
	if not exist IDL mkdir IDL
	xcopy /s /q /y ..\java\IDL IDL\ 
	cd IDL
	copy ..\..\..\..\IDLToCLSCompiler\IDL\orb.idl .
	..\..\..\..\IDLToCLSCompiler\IDLCompiler\bin\IDLToCLSCompiler.exe -o ..\bin adderClient ch\elca\iiop\demo\ejbAdder\Adder.idl ch\elca\iiop\demo\ejbAdder\AdderHome.idl
	cd ..
	


build-client:
	copy ..\..\..\IIOPChannel\bin\IIOPChannel.dll bin\ 
	$(CSC) /r:bin\IIOPChannel.dll /r:bin\adderClient.dll /t:exe /out:bin\Client.exe Client.cs ExceptionImpl.cs
	




clean:
	if exist bin del /s /q bin\ 
	if exist bin rmdir /s /q bin\ 
	if exist IDL del /s /q IDL\ 
	if exist IDL rmdir /s /q IDL\ 

	