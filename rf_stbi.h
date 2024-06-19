#ifndef _REFUEL_STBI_H_
#define _REFUEL_STBI_H_

#include "docker-cross-compiler/include/rf_common.h"

#define STBIDEF RFEXTERN
#define STBI_NO_THREAD_LOCALS 1
#include "stb/stb_image.h"

#define STBIWDEF RFEXTERN
#define STBI_WRITE_NO_STDIO 1
#include "stb/stb_image_write.h"

#endif
