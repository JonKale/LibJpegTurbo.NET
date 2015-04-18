// TurboJpegHarness.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int wmain(int argc, wchar_t* argv[])
{
    int count;
    wchar_t buffer[256];

    tjscalingfactor *scalingFactors = tjGetScalingFactors(&count);
    StringCchPrintf(buffer, 256, L"Got %d scaling factors\n", count);
    OutputDebugString(buffer);

    for (int i = 0; i < count; ++i)
    {
        tjscalingfactor sf = scalingFactors[i];
        StringCchPrintf(buffer, 256, L"Scaling factor %d is %d/%d\n", i, sf.num, sf.denom);
        OutputDebugString(buffer);
    }

    return 0;
}

