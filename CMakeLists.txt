cmake_minimum_required(VERSION 2.6)


macro(make_cs VERSION)
    add_custom_target(ltrModulesNet_${VERSION}
        COMMAND cmd /c "if not exist ${VERSION} mkdir ${VERSION}"
        COMMAND cd ${CMAKE_CURRENT_SOURCE_DIR}
        COMMAND ${NET_FRAMEVORK${VERSION}_DIR}/csc /t:library /out:${CMAKE_CURRENT_BINARY_DIR}/${VERSION}/ltrModulesNet.dll /optimize /keyfile:${DOTNET_SIGN_KEY} ${CMAKE_CURRENT_SOURCE_DIR}/*cs
        WORKING_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}
    )
endmacro(make_cs)

make_cs(v2.0)

add_custom_target(ltrModulesNet
    DEPENDS ltrModulesNet_v2.0
    )
