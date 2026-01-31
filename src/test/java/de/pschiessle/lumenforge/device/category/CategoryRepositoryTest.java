package de.pschiessle.lumenforge.device.category;

import de.pschiessle.lumenforge.components.category.Category;
import de.pschiessle.lumenforge.components.category.CategoryRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;

import static org.assertj.core.api.Assertions.assertThat;

@DataJpaTest
class CategoryRepositoryTest {

    @Autowired
    private CategoryRepository categoryRepository;

    @BeforeEach
    void clearDatabase() {
        categoryRepository.deleteAll();
    }

    @Test
    void searchBySimilarityMatchesNameAndDescriptionAndOrdersByPrefix() {
        Category cutter = new Category();
        cutter.setName("Laser Cutter");
        cutter.setDescription("Precision cutting equipment");
        categoryRepository.save(cutter);

        Category laserArray = new Category();
        laserArray.setName("Laser Array");
        laserArray.setDescription("Array of laser modules");
        categoryRepository.save(laserArray);

        Category engraving = new Category();
        engraving.setName("Engraving Tool");
        engraving.setDescription("Laser-based engraving");
        categoryRepository.save(engraving);

        Category misc = new Category();
        misc.setName("Misc");
        misc.setDescription("Spare parts");
        categoryRepository.save(misc);

        Page<Category> results = categoryRepository.searchBySimilarity("laser", PageRequest.of(0, 10));
        assertThat(results.getContent())
                .containsExactly(laserArray, cutter, engraving);
    }

    @Test
    void searchBySimilarityReturnsEmptyForNoMatches() {
        Page<Category> results = categoryRepository.searchBySimilarity("missing", PageRequest.of(0, 10));
        assertThat(results.getContent()).isEmpty();
    }

    @Test
    void searchBySimilaritySupportsPaginationAcrossMultipleMatches() {
        for (int i = 1; i <= 8; i++) {
            Category category = new Category();
            category.setName("Laser Unit " + i);
            category.setDescription("Laser series " + i);
            categoryRepository.save(category);
        }

        Category descriptionOnly = new Category();
        descriptionOnly.setName("Assembly");
        descriptionOnly.setDescription("laser integration");
        categoryRepository.save(descriptionOnly);

        Page<Category> firstPage = categoryRepository.searchBySimilarity("laser", PageRequest.of(0, 5));
        Page<Category> secondPage = categoryRepository.searchBySimilarity("laser", PageRequest.of(1, 5));

        assertThat(firstPage.getContent()).hasSize(5);
        assertThat(secondPage.getContent()).hasSize(4);
        assertThat(firstPage.getTotalElements()).isEqualTo(9);
        assertThat(secondPage.getContent())
                .allMatch(category -> category.getName().toLowerCase().contains("laser")
                        || category.getDescription().toLowerCase().contains("laser"));
    }
}