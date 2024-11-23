.assembly extern mscorlib {}
.assembly toyprogram
{
	.ver 1:0:1:0
}

.method static void main() cil managed
{
	.maxstack 128
	.entrypoint

	// Decalring Variable x 
	.locals init(float32 x)

	ldc.r8 5.22
	stloc x

	ldloc x
	call void [mscorlib]System.Console::Write(float32)
	ldstr "Hello"
	call void [mscorlib]System.Console::Write(string)

	ret
}
