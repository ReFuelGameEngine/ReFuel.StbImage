#ifndef _QUIK_STBI_H_
#define _QUIK_STBI_H_

#include "Quik.Common/include/quik_common.h"

#define STBIDEF QEXTERN

#define STBI_NO_THREAD_LOCALS 1
#define STBI_NO_FAILURE_STRINGS 1
#define STBI_THREAD_LOCAL
#include "stb/stb_image.h"

#endif
