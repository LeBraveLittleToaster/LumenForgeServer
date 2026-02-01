package de.pschiessle.lumenforge.components.vendor.swagger;

import java.util.List;

public record VendorGetAllResponseDTO(
        List<VendorResponseDTO> content,
        Integer pageNumber,
        Integer pageSize,
        Long totalElements,
        Integer totalPages
) {}
