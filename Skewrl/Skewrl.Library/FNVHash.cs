using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skewrl.Library
{
    //This is a partial C# implementation of hash algorithm by Fowler/Noll/Vo
    //http://www.isthe.com/chongo/tech/comp/fnv/index.html
   public static class FNVHash
    {
       private static ulong FNV_Prime = 1099511628211;
       private static ulong Offest_basis = 14695981039346656037;

       private static uint FNV_32_PRIME = 0x01000193; //Which is Integer 16777619
       private static uint FNV1_32_INIT = 0x811c9dc5; //Which is Integer 2166136261

       /// <summary>
       /// Generates a 32-bit hash for the given string
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static uint fnv_32a_str(String str)
       {
           uint hash = FNV1_32_INIT;
           foreach (char c in str)
           {
               /* xor the bottom with the current octet */
               hash ^= (uint)c;

               /* multiply by the 64 bit FNV magic prime mod 2^32 */
               hash += (hash << 1) + (hash << 4) + (hash << 7) + (hash << 8) + (hash << 24);
           }

           return hash;
       }


       /// <summary>
       /// Generates a 64-bit hash for the given string
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static ulong fnv_64a_str(String str)
       {
           ulong hash = Offest_basis;
           foreach (char c in str)
           {
               hash ^= (ulong)c;

               /* multiply by the 64 bit FNV magic prime mod 2^64 */
               hash += (hash << 1) + (hash << 4) + (hash << 5) + (hash << 7) + (hash << 8) + (hash << 40);
           }

           return hash;
       }
    }
}
