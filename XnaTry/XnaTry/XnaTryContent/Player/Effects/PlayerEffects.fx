sampler s0;

///<summery>
/// Draws a player as a "Ghost" if he is dead
///</summery>
float4 DeathDraw(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// If the player is dead and the pixel isn't transparant
	if (color.a)
	{
		// Negative
		color.rgb =  1 - coords.y;

		// Transparnt
		color.a = 0;
	}

    return color;
}

///<summery>
/// When Damaged turn to red
///</summery>
float4 GotHit(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// If the player is dead and the pixel isn't transparant
	if (color.a)
	{
		color.r += 0.6;
		color.gb -= 0.6;
	}

    return color;
}

///<summery>
/// When Damaged turn to red
///</summery>
float4 GotHealed(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// If the player is dead and the pixel isn't transparant
	if (color.a)
	{
		color.g += 0.6;
		color.rb -= 0.6;
	}

    return color;
}

technique Technique1
{
    pass Ghost
    {
        PixelShader = compile ps_2_0 DeathDraw();
    }
	pass Hit
    {
        PixelShader = compile ps_2_0 GotHit();
    }
	pass Healed
    {
        PixelShader = compile ps_2_0 GotHealed();
    }
}
